using UnityEngine;
using System.Collections;

public class LevelGenerator : MonoBehaviour {

    public GameObject FloorPrefab;
    public GameObject WallPreFab;
    public GameObject Player0;
    public GameObject[] Decorations;

    public int SmallestRoomSize;
    public int DoorOpening;

    public ISpawner[] GameObjectSpawners;

    private GameObject[] CornerTiles;

	// Use this for initialization
	public void GenerateLevel(int width, int height) 
    {
        Random.seed=System.DateTime.UtcNow.Millisecond;
        //CornerTiles = new GameObject[4];
	    //Make a Large Room and begin dividing it up
        SpriteRenderer floorSprite = FloorPrefab.GetComponent<SpriteRenderer>();
        Vector3 origin = new Vector3(0,0,0);
        //Probably shouldn't take only 1 component, right?
        float startingBounds = floorSprite.bounds.size.x; //For now, we'll assume our sprites are squares.
        //HACK: We always want to get even numbered tiles to avoid rounding errors. Not enough time (or sleep) to do good math.
        int MapSizeX = width;
        int MapSizeY = height;

        Grid grid = GetComponent<Grid>();
        if (grid == null)
        {
            return;
        }

        Vector3 basePosition = gameObject.transform.position + Vector3.left * (float)MapSizeX / 2.0f + Vector3.down * (float)MapSizeY / 2.0f + new Vector3(0.5f, 0.5f);

        GameObject tempFloorTile;
        for (int j = 0; j < MapSizeY; j++ )
        {
            for (int i = 0; i < MapSizeX; i++)
            {
                origin.x = i;
                origin.y = j;
                tempFloorTile = (GameObject)GameObject.Instantiate(FloorPrefab, origin + basePosition, Quaternion.identity);
                grid.SetTile(tempFloorTile, new TileCoord(i, j));
            }
        }

        //Figure out your 4 corners before placing walls
        Vector3 top = new Vector3(transform.position.x, basePosition.y + MapSizeY - 0.5f);
        Vector3 bottom = new Vector3(transform.position.x, basePosition.y - 0.5f); // Will want to add another sort of wall with Incorporated door

        Vector3 right = new Vector3(basePosition.x + MapSizeX - 0.5f, top.y - MapSizeY * 0.5f);
        Vector3 left = new Vector3(basePosition.x - 0.5f, right.y);


        //Put all the outer walls
        GameObject top1 = (GameObject)GameObject.Instantiate(WallPreFab, top, Quaternion.AngleAxis(90, Vector3.forward));
        top1.transform.localScale = new Vector3(1, MapSizeX * startingBounds, 1);
        top1.transform.parent = transform;
        top1.name = "top1";

        GameObject bottom1 = (GameObject)GameObject.Instantiate(WallPreFab, bottom, Quaternion.AngleAxis(90, Vector3.forward));
        bottom1.transform.localScale = new Vector3(1, MapSizeX * startingBounds, 1);
        bottom1.transform.parent = transform;
        bottom1.name = "bottom1";

        GameObject left1 = (GameObject)GameObject.Instantiate(WallPreFab, left, Quaternion.identity);
        left1.transform.localScale = new Vector3(1, MapSizeY * startingBounds, 1);
        left1.name = "left1";
        left1.transform.parent = transform;

        GameObject right1 = (GameObject)GameObject.Instantiate(WallPreFab, right, Quaternion.identity);
        right1.transform.localScale = new Vector3(1, MapSizeY * startingBounds, 1);
        right1.name = "right1";
        right1.transform.parent = transform;

        //Now, go into BSP
        BinarySpacePartition(basePosition, 0, 0, (int)MapSizeX, (int)MapSizeY, 4, 0);

        for (int i = 0; i < GameObjectSpawners.Length; ++i)
        {
            GameObjectSpawners[i].Spawn(grid);
        }
	}

    // Update is called once per frame
    void Update()
    {
	
	}
    /// <summary>
    /// Recursive Binary Space Division of a rectangular box
    /// Guaranteed room size to never be below SmallestRoomSize
    /// Takes x1,y1 and x2,y2 to build rectangle, cuts it in the middle on X or Y
    /// Ends when MaxSubs are met OR when rooms would come out to be smaller than SmallestRoomSize
    /// x1y1 corresponds to lower-left corner of rectangle
    /// x2y2 corresponds to upper-right
    /// </summary> 
    private void BinarySpacePartition(Vector3 basePosition, int x1, int y1, int x2, int y2, int maxLevels, int currentLevel)
    {
        //TODO: FIX POSITION
            //Rooms are generated very eratically, becomes difficult to work with.
        //Debug.Log(string.Format("x1: {0} x2: {1} y1: {2} y2: {3} Levels: {4}", x1, x2, y1, y2, maxLevels, currentLevel));
        if(currentLevel == maxLevels)
        {
            //Debug.LogWarning("Level Generation Complete due to Max Recursion");
            return;
        }

        Grid grid = GetComponent<Grid>();
        if (grid == null)
        {
            return;
        }

        //Choose: side=<50 => X OR side=>50 => Y
        int axis = Random.Range(0,100); // 50/50 chance, right?
        int xLength = Mathf.Abs(x1 - x2);
        int yLength = Mathf.Abs(y1 - y2);

        int newMiddle = 0;
        //Debug.log("side:" + axis);

        //Before we begin, evaluate certain decisions ahead of time.
        //1. Do we want to really, really split this room?
        if(currentLevel >= 0 && currentLevel >= maxLevels*(2/3) && Mathf.Min(xLength,yLength) < SmallestRoomSize*2)
        {
            if (Random.Range(0, 100) > 90) //10% Chance that this Room will not be split.
            {
                //Debug.logWarning("Level Generation Complete due to Random Decision");
                return;
            }
        }

        //2. Do we want to change our split side decision?
        if (xLength <= (SmallestRoomSize * 2))
        {
            if (yLength > SmallestRoomSize * 2)
            {
                axis = 100; //Force a choice on the Y Axis
            }
            else
            {
                return;
            }
        }
        if (yLength <= (SmallestRoomSize * 2))
        {
            if (xLength > SmallestRoomSize * 2)
            {
                axis = 0; //Force a choice on the X axis
            }
            else
            {
                return;
            }

        }

        //Time to cut and make walls
        if (axis <= 50) //We "Chose" X-Axis
        {
            //Debug.log("X-Axis halve");
            //We keep x1 the same, x2 will change, however
            //newMiddle = (int)((x2-x1) * (Random.Range(0.3,0.8));
            newMiddle = Mathf.FloorToInt((x2 - x1) / 2);
			newMiddle += Mathf.FloorToInt(Random.Range(-newMiddle/2,newMiddle/2));
            int midPoint = x2 - newMiddle;
            //newMiddle = (int)(Random.Range(MinMapTiles*2, x2 - (MinMapTiles*2)));
            BinarySpacePartition(basePosition, x1, y1, midPoint, y2, maxLevels, currentLevel + 1);
            BinarySpacePartition(basePosition, midPoint, y1, x2, y2, maxLevels, currentLevel + 1);
            GameObject unitWall;

            //Choose a point to make the Door Opening
            int doorIndex = Random.Range(0, yLength - DoorOpening);

            //Build ALL of the Walls as you return from callstack
            for (int i = 0; i < yLength; i++)
            {
                if (i == doorIndex)
                {
                    i += DoorOpening - 1;
                    continue;
                }

                TileCoord coord = new TileCoord(midPoint, y2 - i - 1);
                unitWall = (GameObject)GameObject.Instantiate(WallPreFab, new Vector3(coord.X - 0.5f, coord.Y) + basePosition
                    , Quaternion.identity);
                unitWall.name = string.Format("{0}/{1}", i + 1, yLength);
                unitWall.transform.parent = transform;
                    
                TileCoord otherCoord = new TileCoord(coord.X - 1, coord.Y);

                grid.AddWall(coord, otherCoord, unitWall);
            }

        }
        else if (axis >= 50) //We "Chose" Y-Axis
        {
            //Debug.log("Y-Axis halve");
            //newMiddle = (int)(y2 / ((Random.value % 2) + 1));
            newMiddle = (y2 - y1) / 2;
			newMiddle += Mathf.FloorToInt(Random.Range(-newMiddle / 2, newMiddle / 2));
            int midPoint = y2 - newMiddle;
            //newMiddle = (int)(Random.Range(MinMapTiles*2, y2 - (MinMapTiles*2)));
            BinarySpacePartition(basePosition, x1, y1, x2, midPoint, maxLevels, currentLevel + 1);
            BinarySpacePartition(basePosition, x1, midPoint, x2, y2, maxLevels, currentLevel + 1);
            GameObject unitWall;

            //Choose a point to make the Door Opening
            int doorIndex = Random.Range(0, yLength - DoorOpening);

            //Build ALL of the Walls as you return from callstack
            for (int i = 0; i < xLength; i++)
            {
                if (i == doorIndex)
                {
                    i += DoorOpening-1;
                    continue;
                }

                TileCoord coord = new TileCoord(x2 - i - 1, midPoint);

                unitWall = (GameObject)GameObject.Instantiate(WallPreFab, new Vector3(coord.X, coord.Y - 0.5f) + basePosition
                    , Quaternion.AngleAxis(90, Vector3.forward));
                unitWall.transform.parent = transform;

                TileCoord otherCoord = new TileCoord(coord.X, coord.Y - 1);

                grid.AddWall(coord, otherCoord, unitWall);
            }
        }

        


        //If we are returning from other methods, create the walls

        //Finish by 
    }
}
