using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
    public int Width = 10;
    public int Height = 10;

    public int MaxWidth = 10;
    public int MaxHeight = 10;

    private static Grid _instance;
    public static Grid Instance 
    {
        get
        {
            return _instance;
        }
    }

    public List<List<Tile>> m_Tiles = new List<List<Tile>>();

    public List<GameObject> m_Players = new List<GameObject>();
    public List<GameObject> m_Civilian = new List<GameObject>();

    public void Awake()
    {
        _instance = this;
    }

    public void Start()
    {
        PlayerContainer.Instance.level++;

        Width = Random.Range(Width, MaxWidth);
        Height = Random.Range(Height, MaxHeight);

        for (int j = 0; j < Width; ++j)
        {
            m_Tiles.Add(new List<Tile>());

            for (int i = 0; i < Height; ++i)
            {
                m_Tiles[m_Tiles.Count - 1].Add(null);
            }
        }

        LevelGenerator lg = GetComponent<LevelGenerator>();
        if(lg != null)
        {
            lg.GenerateLevel(Width, Height);
        }
    }

    public void SetTile(GameObject tileObject, TileCoord coord)
    {
        Tile tileComponent = GameObjectUtility.GetOrAddComponent<Tile>(tileObject);
		tileComponent.Initialize(this, coord);

        m_Tiles[coord.X][coord.Y] = tileComponent;
        tileObject.transform.parent = transform;
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

    public Tile GetTileAtPosition(Vector3 position)
    {
        Vector3 basePosition = gameObject.transform.position + Vector3.left * (float)Width / 2.0f + Vector3.down * (float)Height / 2.0f + new Vector3(0.5f, 0.5f, 0.0f);

        Vector3 vectorCoord = position - basePosition;
        TileCoord coord = new TileCoord(Mathf.RoundToInt(vectorCoord.x), Mathf.RoundToInt(vectorCoord.y));

        return GetTile(coord);
    }

    public List<Tile> GetConnectedTileInRange(Tile tile, int range)
    {
        return GetNConnectedTileInRange(tile, range, -1);
    }

    public List<Tile> GetNConnectedTileInRange(Tile tile, int range, int number)
    {
        HashSet<Tile> connectingTiles = new HashSet<Tile>();

		List<Tile> neighbors = new List<Tile>(){tile};
        List<Tile> newNeighbors = new List<Tile>();

        for (int i = 0; i < neighbors.Count; ++i)
        {
            connectingTiles.Add(neighbors[i]);
            if (connectingTiles.Count == number)
            {
                return new List<Tile>(connectingTiles);
            }
        }

        for (int r = 0; r < range; ++r)
        {
            newNeighbors.Clear();
            while (neighbors.Count > 0)
            {
                List<Tile> currentNeighbors = neighbors[0].GetNeighboors();
                for (int j = 0; j < currentNeighbors.Count; ++j)
                {
					if (!connectingTiles.Contains(currentNeighbors[j]) && currentNeighbors[j] != tile)
                    {
						if(neighbors[0].IsConnectedTo(currentNeighbors[j]))
						{
                            newNeighbors.Add(currentNeighbors[j]);
                        }
                    }
                }

                for (int i = 0; i < newNeighbors.Count; ++i)
                {
                    connectingTiles.Add(newNeighbors[i]);
                    if (connectingTiles.Count == number)
                    {
                        return new List<Tile>(connectingTiles);
                    }
                }
                neighbors.RemoveAt(0);
            }

			neighbors.Clear();
			neighbors.AddRange(newNeighbors);
        }

        return new List<Tile>(connectingTiles);
    }
}
