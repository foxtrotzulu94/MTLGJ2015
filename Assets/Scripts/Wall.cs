using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour
{
    public Tile A;
    public Tile B;

    public void Initialize(Tile a, Tile b)
    {
        A = a;
        B = b;
    }

    public void Apply()
    {
        A.AddWall(this);
        B.AddWall(this);
    }

    public void Delete()
    {
        A.RemoveWall(this);
        B.RemoveWall(this);
    }

    public bool IsBetween(Tile a, Tile b)
    {
        return (A == a && B == b) || (A == b && B == a);
    }

	public void OnDestroy()
	{
		A.RemoveWall (this);
		B.RemoveWall (this);
	}
}
