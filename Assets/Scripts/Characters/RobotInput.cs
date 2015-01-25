using System;
using UnityEngine;
using System.Collections;

public class RobotInput : MonoBehaviour
{
    Vector2 mouseLocation;
    Collider2D ourBounds;
    public bool isFocus;
    public bool isAlive { get { return alive; } }
    public RobotType Type;
	Animator animator;
	bool facingRight = true;
    public int civiliansRescued;
    //Water player stuff
    public GameObject Water;
    public float CooldownDuration = 3.0f;
    public GameObject SelectedPrefab;
    private GameObject m_SelectedObject;

    //Breaker player stuff
    public int DestroyCharge = 3;

    public enum RobotType : int
    {
        Waterer = 1,
        Pusher = 2,
        Breaker = 3
    }

    private bool alive;
    private bool inLevel;
    private PlayerMovement Main;

 // Use this for initialization
    void Start () 
    {
        Main = Camera.main.GetComponent<PlayerMovement>();
        mouseLocation = gameObject.transform.position;
        ourBounds = GetComponent<BoxCollider2D>();
		animator = GetComponent<Animator> ();
        inLevel = true;
        alive = true;

        m_SelectedObject = (GameObject)GameObject.Instantiate(SelectedPrefab, transform.position + Vector3.up * 0.25f, Quaternion.identity);
        m_SelectedObject.SetActive(false);
        m_SelectedObject.transform.parent = transform;
    }
 
 // Update is called once per frame
    float m_SelectionTime = 0.0f;
    void Update () 
    {
        Cooldown -= TimeManager.GetTime(TimeType.Gameplay);
        if (isFocus && inLevel && alive)
        {
            //LookAtMouse();

            LeftClickAction();

            RightClickAction();
        }

        m_SelectionTime += TimeManager.GetTime(TimeType.Engine) * 5.0f;

        if (m_SelectedObject.active)
        {
            float ratio = (Mathf.Sin(m_SelectionTime) + 1) * 0.5f;
            m_SelectedObject.transform.localPosition = Vector3.up * (0.25f + ratio * 0.2f) + Vector3.back * 0.1f;
			m_SelectedObject.transform.localScale = new Vector3 (0.2f, 0.2f, 1.0f);
        }
    }

    public void ShowSelector()
    {
        m_SelectedObject.SetActive(true);
    }

    public void HideSelector()
    {
        m_SelectedObject.SetActive(false);
    }

    private void LeftClickAction()
    {
        //Left Click - Movement
        if (Input.GetMouseButtonUp(0))
        {
            mouseLocation = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        }

        float distanceToGoal = (mouseLocation - new Vector2(gameObject.transform.position.x, gameObject.transform.position.y)).magnitude;

        if (distanceToGoal > 0.01f && alive)
        {
            HideSelector();
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, mouseLocation,
                2.0f*TimeManager.GetTime(TimeType.Gameplay));

			float dirX = Mathf.Abs(mouseLocation.x - transform.position.x);
			float dirY = Mathf.Abs(mouseLocation.y - transform.position.y);
			if (dirX > dirY)
			{
				if ((mouseLocation.x - transform.position.x) > 0)
					animator.SetInteger ("transition", 0);
				else
					animator.SetInteger ("transition", 1);
				if ((mouseLocation.x - transform.position.x) < 0 && facingRight) {
					Flip();
				}
				if ((mouseLocation.x - transform.position.x) > 0 && !facingRight) {
					Flip();
				}
			}
			else {
				if ((mouseLocation.y - transform.position.y) > 0)
					animator.SetInteger ("transition", 2);
				else
					animator.SetInteger ("transition", 3);
			}
        }
    }

    float Cooldown = 0.0f;

    private void RightClickAction()
    {
        if(Cooldown <= 0.0f)
        {
            if (Input.GetMouseButtonUp(1))
            {
                Vector3 mouseLocation2 =
                        Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
                Vector3 dir = (new Vector2(mouseLocation2.x - transform.position.x, mouseLocation2.y - transform.position.y)).normalized;

                if (Type == RobotType.Waterer)
                {
                    GameObject projectileGameObject = Instantiate(Water, transform.position + dir * 0.5f, Quaternion.identity) as GameObject;
                    SquirtTimer squirt = projectileGameObject.AddComponent<SquirtTimer>();
                    squirt.direction = dir;
                    Cooldown = CooldownDuration;
                }
                else if (Type == RobotType.Breaker)
                {
                    var layerMask = 1 << LayerMask.NameToLayer("Wall");
                    RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), new Vector2(dir.x, dir.y), 1.0f, layerMask);
                    if (hit.collider != null)
                    {
                        Destroy(hit.collider.gameObject);
                        //DestroyCharge--;
						Cooldown = CooldownDuration;
                    }
                }
                else if (Type == RobotType.Pusher)
                {
                    //Not sure what goes here/
                    Cooldown = CooldownDuration;
                }
            }
        }
    }

    private void LookAtMouse()
    {
        //Always Look towards the mouse.
        Vector3 mouseLocation = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));

        //// Get Angle in Radians
        float AngleRad = Mathf.Atan2(mouseLocation.y - transform.position.y, mouseLocation.x - transform.position.x);
        // Get Angle in Degrees
        float AngleDeg = (180/Mathf.PI)*AngleRad;
        // Rotate Object
        transform.rotation = Quaternion.Euler(0, 0, AngleDeg);
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        /*if (collider.gameObject.tag == "Wall"  && Type == RobotType.Breaker)
        {
            Destroy(collider.gameObject);
            DestroyCharge--;
        }*/
        if (collider.gameObject.tag == "Props")
        {
            if (Type == RobotType.Pusher)
                collider.gameObject.rigidbody2D.isKinematic = false;
            else
                collider.gameObject.rigidbody2D.isKinematic = true;
        }

    }

	void Kill()
	{
		transform.position = new Vector3 (2.644737f, -0.9473684f, 0);	//spawn zone
		renderer.enabled = false;
	    alive = false;
        Main.RegisterKilledRobot(this);
        //Respawn();
	}

	void Respawn()
	{
	    gameObject.AddComponent("Respawn");
	}

    void Revived()
    {
        mouseLocation = transform.position;
        renderer.enabled = true;
        alive = true;
    }

	void Flip()
	{
		if (TimeManager.GetTime(TimeType.Gameplay) > 0.0f)
		{
			facingRight = !facingRight;
			Vector3 nextScale = transform.localScale;
			nextScale.x *= -1f;
			transform.localScale = nextScale;
		}
	}
	
	float Orient(float i) {
		return facingRight ? -i : i;
	}

    public void SendToNextLevel()
    {
        this.GetComponent<SpriteRenderer>().enabled=false;
        inLevel = false;
        Main.RegisterSafeRobot(this);
    }

    public void AddRescuedCivilian(Civilian aCivilian)
    {
        civiliansRescued++;
    }
}
