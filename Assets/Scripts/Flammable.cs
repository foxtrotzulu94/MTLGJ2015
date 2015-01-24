using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;

public enum FlammableState
{
	NotOnFire,
	OnFire,
	SuperOnFire,
}

public class Flammable : MonoBehaviour
{
	public float FlameResistance = 100.0f;
	public GameObject FireObject = null;
    public bool WaitForEventSyncToIgnite = false;

    public Vector2 HeatSpreadingRange = new Vector2(0.0f, 10.0f);

	public FlammableState m_State;

    private Tile m_Tile;

    public void Start()
    {
        m_Tile = gameObject.GetComponent<Tile>();

        if (m_State == FlammableState.OnFire)
        {
            Ignite();
        }
    }

	public void Update()
	{
		if (m_State != FlammableState.NotOnFire)
        {
            List<Tile> neighbors = m_Tile.GetConnectedNeighboors();
            for (int i = 0; i < neighbors.Count; ++i)
            {
                Flammable f = neighbors[i].GetComponent<Flammable>();
                if (f != null)
                {
                    f.ReceiveHeat(UnityEngine.Random.Range(HeatSpreadingRange.x, HeatSpreadingRange.y) * Time.deltaTime);
                }
            }
		}
	}

    public void ReceiveHeat(float heatAmount)
    {
        FlameResistance -= heatAmount;

        if (FlameResistance <= 0.0f && !WaitForEventSyncToIgnite)
        {
            Ignite();
        }
    }

    public void Ignite()
    {
        FlameResistance = 0.0f;
        if (m_State == FlammableState.NotOnFire)
        {
            m_State = FlammableState.OnFire;
            GameObject fireObject = (GameObject)GameObject.Instantiate(FireObject);

            fireObject.transform.position = transform.position;
            fireObject.transform.parent = transform;
            fire fireComp = GameObjectUtility.GetOrAddComponent<fire>(fireObject);

            fireComp.Initialize(this);
        }
    }

    public void SuperSize()
    {
        if (m_State == FlammableState.OnFire)
        {
            m_State = FlammableState.SuperOnFire;
        }
    }

    public void OnDrawGizmos()
    {
		Handles.Label(transform.position + Vector3.left * 0.5f, String.Format("{0:0.##}", FlameResistance));

		if (m_State == FlammableState.OnFire || m_State == FlammableState.SuperOnFire)
        {
            m_Tile.GizmosDrawWalls(0.5f, false, Color.red);
		}
    }
}
