using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
    public int Width = 10;
    public int Height = 10;

    public GameObject TilePrefab;
    public GameObject WallPrefab;

    private Grid _instance;
    public Grid Instance 
    {
        get
        {
            return _instance;
        }
    }

    private List<List<Tile>> m_Tiles = new List<List<Tile>>();

    public void Awake()
    {
        _instance = this;
    }

	// Use this for initialization
	void Start ()
    {
        for (int j = 0; j < Height; ++j)
        {
            m_Tiles.Add(new List<Tile>());

            for (int i = 0; i < Width; ++i)
            {
                m_Tiles[m_Tiles.Count - 1].Add(null);
            }
        }

        Vector3 basePosition = gameObject.transform.position + Vector3.left * (float)Width / 2.0f + Vector3.down * (float)Height / 2.0f;

        for (int j = 0; j < Height; ++j)
        {
            m_Tiles.Add(new List<Tile>());

            for (int i = 0; i < Width; ++i)
            {
                GameObject newTile = (GameObject)GameObject.Instantiate(TilePrefab);
                newTile.transform.parent = gameObject.transform;

                Tile tileComponent = newTile.GetComponent<Tile>();
                if (tileComponent == null)
                {
                    tileComponent = newTile.AddComponent<Tile>();
                }

                tileComponent.Initialize(this, new TileCoord(i, j));

                newTile.transform.position = basePosition + new Vector3(i, j, 0);

                m_Tiles[i][j] = tileComponent;
            }
        }

        // @TEST
        /*TileCoord fireCoord = new TileCoord(Random.Range(0, Width), Random.Range(0, Height));
        Tile tile = GetTile(fireCoord);
        Flammable flamabble = tile.gameObject.GetComponent<Flammable>();
        if (flamabble != null)
        {
            flamabble.Ignite();
        }

        for (int i = 0; i < 150; ++i)
        {
            fireCoord = new TileCoord(Random.Range(0, Width), Random.Range(0, Height));
            Direction randomDir = (Direction)Random.Range(0, 4);
            tile = GetTile(fireCoord);

            Tile neighbor = tile.GetNeighboor(randomDir);

            if (neighbor != null)
            {
                if (tile.GetConnectedNeighboors().Contains(neighbor))
                {
                    GameObject wall = (GameObject)GameObject.Instantiate(WallPrefab);
                    wall.transform.position = (tile.transform.position + neighbor.transform.position) * 0.5f;
                    wall.transform.parent = transform;
                    if (randomDir == Direction.Up || randomDir == Direction.Down)
                    {
                        wall.transform.Rotate(Vector3.forward, 90.0f);
                    }
                    AddWall(tile, neighbor, wall);
                }
            }
            else
            {
                ++i;
            }
        }*/
    }

    public Tile GetTile(int x, int y)
    {
        if (x >= 0 && x < Width && y >= 0 && y < Height)
        {
            return m_Tiles[x][y];
        }

        return null;
    }

    public Tile GetTile(TileCoord coord)
    {
        return GetTile(coord.X, coord.Y);
    }

    public List<Tile> GetNeighboors(Tile tile)
    {
        return GetNeighboors(tile.m_Coord);
    }

    public List<Tile> GetNeighboors(TileCoord coord)
    {
        List<Tile> neighboors = new List<Tile>();

        if (coord.X < Width - 1)
        {
            neighboors.Add(GetTile(coord.X + 1, coord.Y));
        }

        if (coord.X > 0)
        {
            neighboors.Add(GetTile(coord.X - 1, coord.Y));
        }

        if (coord.Y < Height - 1)
        {
            neighboors.Add(GetTile(coord.X, coord.Y + 1));
        }

        if (coord.Y > 0)
        {
            neighboors.Add(GetTile(coord.X, coord.Y - 1));
        }

        return neighboors;
    }

    public void AddWall(Tile A, Tile B, GameObject wallObject)
    {
        Wall wall = wallObject.GetComponent<Wall>();
        if (wall == null)
        {
            wall = wallObject.AddComponent<Wall>();
        }
        wall.Initialize(A, B);

        wall.Apply();
    }

    public void AddWall(TileCoord A, TileCoord B, GameObject gameObject)
    {
        AddWall(GetTile(A), GetTile(B), gameObject);
    }

	//@TEST
    /*float timer = 3.0f;
	public void Update()
	{
        timer -= TimeManager.GetTime(TimeType.Gameplay);
		if(timer <= 0.0f)
		{
			BroadcastMessage("SpecialActionRegistrationEvent");
            SpecialEventManager.Instance.ExecuteSpecialAction(1);
            timer = 3.0f;
		}

		if(Input.GetKeyUp(KeyCode.Space))
		{
            if (TimeManager.GetTimeDilatation(TimeType.Gameplay) <= 0.0f)
            {
                TimeManager.SetTimeDilatation(TimeType.Gameplay, 1.0f);
            }
            else
            {
                TimeManager.SetTimeDilatation(TimeType.Gameplay, 0.0f);
            }
		}
	}*/
}
