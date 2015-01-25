using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tile : MonoBehaviour
{
    public Grid m_ParentGrid;
    public TileCoord m_Coord;

    List<WallInstance> m_Walls = new List<WallInstance>();

    public void Initialize(Grid parentGrid, TileCoord coord)
    {
        m_ParentGrid = parentGrid;
        m_Coord = coord;
        gameObject.name = "Tile " + m_Coord.ToString();
    }

    public bool HasAWall(Direction dir)
    {
        for (int i = 0; i < m_Walls.Count; ++i)
        {
            if (m_Walls[i].m_InstanceDirection == dir)
            {
                return true;
            }
        }

        return false;
    }

    public bool IsConnectedTo(Tile B)
    {
        if (B != null)
        {
            return !HasAWall(GetDirectionTo(B));
        }

        return false;
    }

    public void GizmosDrawWalls(float ratio, bool drawWallsColor, Color wallColor)
    {
        Gizmos.color = wallColor;

        Vector3 basePosition = transform.position;
        Vector3 TL = basePosition + (Vector3.left * 0.5f + Vector3.up * 0.5f) * ratio;
        Vector3 TR = basePosition + (Vector3.right * 0.5f + Vector3.up * 0.5f) * ratio;
        Vector3 BL = basePosition + (Vector3.left * 0.5f + Vector3.down * 0.5f) * ratio;
        Vector3 BR = basePosition + (Vector3.right * 0.5f + Vector3.down * 0.5f) * ratio;

        if (drawWallsColor)
        {
            if (HasAWall(Direction.Up) || GetNeighboor(Direction.Up) == null)
            {
                Gizmos.color = Color.black;
            }
            else
            {
                Gizmos.color = wallColor;
            }
        }
        if (!drawWallsColor || Gizmos.color != wallColor)
            Gizmos.DrawLine(TL, TR);

        if (drawWallsColor)
        {
            if (HasAWall(Direction.Right) || GetNeighboor(Direction.Right) == null)
            {
                Gizmos.color = Color.black;
            }
            else
            {
                Gizmos.color = wallColor;
            }
        }
        if (!drawWallsColor || Gizmos.color != wallColor)
            Gizmos.DrawLine(TR, BR);

        if (drawWallsColor)
        {
            if (HasAWall(Direction.Down) || GetNeighboor(Direction.Down) == null)
            {
                Gizmos.color = Color.black;
            }
            else
            {
                Gizmos.color = wallColor;
            }
        }
        if (!drawWallsColor || Gizmos.color != wallColor)
            Gizmos.DrawLine(BR, BL);

        if (drawWallsColor)
        {
            if (HasAWall(Direction.Left) || GetNeighboor(Direction.Left) == null)
            {
                Gizmos.color = Color.black;
            }
            else
            {
                Gizmos.color = wallColor;
            }
        }

        if (!drawWallsColor || Gizmos.color != wallColor)
            Gizmos.DrawLine(BL, TL);
        
    }

    public List<Tile> GetNeighboors()
    {
        return m_ParentGrid.GetNeighboors(this);
    }

    public Tile GetNeighboor(Direction dir)
    {
        TileCoord neighboorCoord = m_Coord.GetAdjacentCoord(dir);

        return m_ParentGrid.GetTile(neighboorCoord); ;
    }

    public List<WallInstance> GetAdjacentWalls()
    {
        return m_Walls;
    }

    public List<Tile> GetConnectedNeighboors()
    {
        List<Tile> neighbors = GetNeighboors();
        for(int i = 0; i < neighbors.Count; ++i)
        {
            Tile t = neighbors[i];
            Direction directionTo = GetDirectionTo(t);
            if (HasAWall(directionTo))
            {
                neighbors.RemoveAt(i--);
            }
        }

        return neighbors;
    }

    public void OnDrawGizmos()
    {
		if (m_Coord == null)
		{
			return;
		}
        GizmosDrawWalls(1.0f, true, Color.white);
    }

    public void OnDrawGizmosSelected()
    {
        GizmosDrawWalls(0.9f, true, Color.red);

        List<Tile> neighbors = GetConnectedNeighboors();
        foreach(Tile t in neighbors)
        {
            t.GizmosDrawWalls(0.9f, false, Color.yellow);
        }
    }

    public void AddWall(Wall wall)
    {
        m_Walls.Add(new WallInstance(wall, this));
    }

    public void RemoveWall(Wall wall)
    {
        foreach (WallInstance w in m_Walls)
        {
            if (w.m_Wall == wall)
            {
                m_Walls.Remove(w);
                return;
            }
        }
    }

    public Direction GetDirectionTo(Tile otherTile)
    {
        if (otherTile.m_Coord.X != m_Coord.X)
        {
            if (otherTile.m_Coord.X > m_Coord.X)
            {
                return Direction.Right;
            }
            else
            {
                return Direction.Left;
            }
        }
        else
        {
            if (otherTile.m_Coord.Y > m_Coord.Y)
            {
                return Direction.Up;
            }
            else
            {
                return Direction.Down;
            }
        }
    }
}

public enum Direction
{
    Up,
    Right,
    Down,
    Left,

    Unconnected
}

public class TileCoord
{
    public int X;
    public int Y;

    public TileCoord(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }

    public new string ToString()
    {
        return "(" + X + ", " + Y + ")";
    }

    public TileCoord GetAdjacentCoord(Direction dir)
    {
        TileCoord coord = new TileCoord(X, Y);

        switch (dir)
        {
            case Direction.Down:
                coord.Y -= 1;
                break;
            case Direction.Up:
                coord.Y += 1;
                break;
            case Direction.Left:
                coord.X -= 1;
                break;
            case Direction.Right:
                coord.X += 1;
                break;
            default:
                break;
        }

        return coord;
    }
}

public class WallInstance
{
    public Wall m_Wall;
    public Direction m_InstanceDirection;

    public WallInstance(Wall wall, Tile tileOwner)
    {
        m_Wall = wall;

        Tile otherTile = m_Wall.A == tileOwner ? m_Wall.B : m_Wall.A;
        m_InstanceDirection = tileOwner.GetDirectionTo(otherTile);
    }
}