using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
    public int Width = 10;
    public int Height = 10;

    public GameObject TilePrefab;

    private Grid _instance;
    public Grid Instance 
    {
        get
        {
            return _instance;
        }
    }

    private List<List<Tile>> m_Tiles = new List<List<Tile>>();
    private List<Wall> m_Walls = new List<Wall>();

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
	}

    public Tile GetTile(int x, int y)
    {
        return m_Tiles[x][y];
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

        m_Walls.Add(wall);

        wall.Apply();
    }

    public void AddWall(TileCoord A, TileCoord B, GameObject gameObject)
    {
        AddWall(GetTile(A), GetTile(B), gameObject);
    }

    public void RemoveWall(TileCoord A, TileCoord B)
    {
        RemoveWall(GetTile(A), GetTile(B));
    }

    public void RemoveWall(Tile A, Tile B)
    {
        for (int i = 0; i < m_Walls.Count; ++i)
        {
            if (m_Walls[i].IsBetween(A, B))
            {
                m_Walls[i].Delete();
                m_Walls.RemoveAt(i);

                return;
            }
        }
    }
}
