using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
public enum FlammableState
{
	NotOnFire,
	OnFire,
	SuperOnFire,
	WaitingForDestruction,
}

public class Flammable : MonoBehaviour
{
	public float FlameResistance = 100.0f;
	public GameObject FireObject = null;
    public bool WaitForEventSyncToIgnite = false;
	public bool CanSpreadFire = true;
    public bool IsDestroyedOnIgnition = false;

    public Vector2 HeatSpreadingRange = new Vector2(0.0f, 10.0f);

	public FlammableState m_State;

    private Tile m_Tile;

    public void Start()
    {
        m_Tile = gameObject.GetComponent<Tile>();

        if (IsOnFire())
        {
            Ignite();
        }
    }

    public bool IsOnFire()
    {
        if (m_State == FlammableState.OnFire || m_State == FlammableState.SuperOnFire)
        {
            return true;
        }

        return false;
    }

	public void Update()
	{
		if(CanSpreadFire)
		{
            if (IsOnFire())
	        {
                if (m_Tile != null)
                {
                    float spreadingMultiplier = 1.0f;
                    if (m_State == FlammableState.SuperOnFire)
                    {
                        spreadingMultiplier = 2.5f;
                    }
                    List<Tile> neighbors = m_Tile.GetConnectedNeighboors();
                    for (int i = 0; i < neighbors.Count; ++i)
                    {
                        Flammable f = neighbors[i].GetComponent<Flammable>();
                        if (f != null)
                        {
                            f.ReceiveHeat(UnityEngine.Random.Range(HeatSpreadingRange.x, HeatSpreadingRange.y) * spreadingMultiplier * TimeManager.GetTime(TimeType.Gameplay));
                        }
                    }

                    List<WallInstance> walls = m_Tile.GetAdjacentWalls();
                    for (int i = 0; i < walls.Count; ++i)
                    {
                        Flammable f = walls[i].m_Wall.GetComponent<Flammable>();
                        if (f != null)
                        {
                            f.ReceiveHeat(UnityEngine.Random.Range(HeatSpreadingRange.x, HeatSpreadingRange.y) * TimeManager.GetTime(TimeType.Gameplay));
                        }
                    }
                }
			}
		}
	}

	public void SpecialActionRegistrationEvent()
	{
        if(m_State == FlammableState.WaitingForDestruction)
		{
            Ignite();
		}
        else
        {
		    if (FlameResistance <= 0.0f && WaitForEventSyncToIgnite)
		    {
			    SpecialEventManager.Instance.Register(gameObject);
		    }
        }
	}

    public void SpecialAction()
    {
       	m_State = FlammableState.WaitingForDestruction;
		SpriteRenderer renderer = GetComponent<SpriteRenderer>();
		if(renderer != null)
		{
			renderer.color = Color.red;
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

        if (!IsDestroyedOnIgnition)
        {
            if (m_State == FlammableState.NotOnFire)
            {
                m_State = FlammableState.OnFire;
                GameObject fireObject = (GameObject)GameObject.Instantiate(FireObject);

                fireObject.transform.position = transform.position + new Vector3(UnityEngine.Random.Range(-1.0f, 1.0f), UnityEngine.Random.Range(-1.0f, 1.0f), -0.1f) * 0.1f;
                fireObject.transform.parent = transform;
                fire fireComp = GameObjectUtility.GetOrAddComponent<fire>(fireObject);

                fireComp.Initialize(this);
            }
        }
        else
        {
            Destroy(gameObject);
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
