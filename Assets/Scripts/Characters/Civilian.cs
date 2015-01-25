using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Civilian : MonoBehaviour
{
    public float DecisionDelay = 1.5f;
    float m_ActionTimer = 0.0f;

    Tile m_CurrentTile;
    Vector3 m_DesiredPosition;

    public float Speed = 3.0f;

    private Tile m_LastRoamingTile;


    public void Start()
    {
        m_CurrentTile = Grid.Instance.GetTileAtPosition(transform.position);
        m_DesiredPosition = transform.position;
    }

	void Update ()
    {
        m_ActionTimer -= TimeManager.GetTime(TimeType.Gameplay);
        if (m_ActionTimer <= 0)
        {
            Tile currentTile = Grid.Instance.GetTileAtPosition(transform.position);

            // Do Action
            List<Tile> tilesInRange = Grid.Instance.GetConnectedTileInRange(currentTile, 3);
            List<Vector3> firePositions = new List<Vector3>();

            for (int i = 0; i < tilesInRange.Count; ++i)
            {
                Flammable flammable = tilesInRange[i].GetComponent<Flammable>();
                if (flammable.IsOnFire())
                {
                    firePositions.Add(flammable.transform.position);
                }
            }

            if (firePositions.Count > 0)
            {
                Vector3 safestDirection = Vector3.zero;
                for (int i = 0; i < firePositions.Count; ++i)
                {
                    Vector3 awayFromFire = Vector3.Normalize(transform.position - firePositions[i]);

                    safestDirection += awayFromFire;
                }

                safestDirection /= (float)firePositions.Count;

				Tile safeNeighbor = null;
				List<Direction> triedDirection = new List<Direction>();

				while(safeNeighbor == null && triedDirection.Count < 4)
				{
					Direction dir = VectorToDirection(safestDirection, triedDirection);
					safeNeighbor = currentTile.GetNeighboor(dir);
					if(!currentTile.IsConnectedTo(safeNeighbor))
					{
						safeNeighbor = null;
					}
                    triedDirection.Add(dir);
				}

                m_DesiredPosition = safeNeighbor.transform.position;
            }
            else
            {
                // Roam
				List<Tile> neighbors = currentTile.GetConnectedNeighboors();
                int randomIndex = Random.Range(0, neighbors.Count);
                Tile selectedNeighbor = neighbors[randomIndex];

                if (neighbors.Count > 1 && selectedNeighbor == m_LastRoamingTile)
                {
                    randomIndex = (randomIndex + 1) % neighbors.Count;
                    selectedNeighbor = neighbors[randomIndex];
                }

                m_DesiredPosition = selectedNeighbor.transform.position + new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)) * 0.2f;
				m_LastRoamingTile = currentTile;
            }

            m_ActionTimer = DecisionDelay;
        }

        Vector3 toDesiredPos = m_DesiredPosition - transform.position;
        float distanceSqrToDesired = toDesiredPos.sqrMagnitude;
        float sqrSpeed = Speed * Speed;

        if (distanceSqrToDesired > sqrSpeed * TimeManager.GetTime(TimeType.Gameplay))
        {
            transform.position += Vector3.Normalize(toDesiredPos) * Speed * TimeManager.GetTime(TimeType.Gameplay);
        }
	}

    private Direction VectorToDirection(Vector3 dir, List<Direction> dirToIgnore)
    {
		List<Direction> orderedDir = new List<Direction> ();

        if (Mathf.Abs(dir.y) > Mathf.Abs(dir.x))
        {
            if (dir.y > 0.0f)
            {
				// Up
                orderedDir.Add(Direction.Up);
                if (dir.x > 0.0f)
                {
                    orderedDir.Add(Direction.Right);
                    orderedDir.Add(Direction.Left);
                }
                else
                {
                    orderedDir.Add(Direction.Left);
                    orderedDir.Add(Direction.Right);
                }
                orderedDir.Add(Direction.Down);
            }
            else
            {
                // Down
                orderedDir.Add(Direction.Down);
                if (dir.x > 0.0f)
                {
                    orderedDir.Add(Direction.Right);
                    orderedDir.Add(Direction.Left);
                }
                else
                {
                    orderedDir.Add(Direction.Left);
                    orderedDir.Add(Direction.Right);
                }
                orderedDir.Add(Direction.Up);
            }
        }
        else
        {
            if (dir.x > 0.0f)
            {
                // Right
                orderedDir.Add(Direction.Right);
                if (dir.y > 0.0f)
                {
                    orderedDir.Add(Direction.Up);
                    orderedDir.Add(Direction.Down);
                }
                else
                {
                    orderedDir.Add(Direction.Down);
                    orderedDir.Add(Direction.Up);
                }
                orderedDir.Add(Direction.Left);
            }
            else
            {
                // Left
                orderedDir.Add(Direction.Left);
                if (dir.y > 0.0f)
                {
                    orderedDir.Add(Direction.Up);
                    orderedDir.Add(Direction.Down);
                }
                else
                {
                    orderedDir.Add(Direction.Down);
                    orderedDir.Add(Direction.Up);
                }
                orderedDir.Add(Direction.Right);
            }
        }

        while(orderedDir.Count > 0)
        {
            if (!dirToIgnore.Contains(orderedDir[0]))
            {
                return orderedDir[0];
            }
            else
            {
                orderedDir.RemoveAt(0);
            }
        }

        // Uh oh
        return Direction.Unconnected;
    }

    public void OnDrawGizmos()
    {
        List<Tile> tilesInRange = Grid.Instance.GetConnectedTileInRange(Grid.Instance.GetTileAtPosition(transform.position), 3);

        for (int i = 0; i < tilesInRange.Count; ++i)
        {
			Gizmos.DrawWireSphere(tilesInRange[i].transform.position, 0.5f);
        }
    }

    public void OnCollisionEnter2D(Collision2D someObject)
    {
        Debug.Log("Civilian Entered Collision");
        RobotInput aRobot = someObject.collider.GetComponentInParent<RobotInput>();
        if (aRobot != null)
        {
            //Save the Civi:
                //Make them disappear, add to the score, Mark for Destroy
            Hide();
        }

    }

    public void OnCollisionStay2D(Collision2D someObject)
    {
        fire theFire = someObject.collider.GetComponentInParent<fire>();
        if (theFire != null)
        {
            Debug.LogWarning("Kill the civi NOW");
            Hide();
        }
    }

    public void Hide()
    {
        this.GetComponent<SpriteRenderer>().enabled = false;
        this.GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        //DestroyObject(this);
    }
}
