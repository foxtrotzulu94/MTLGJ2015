using System;
using UnityEngine;
using System.Collections;

public class RobotInput : MonoBehaviour {
    Vector2 mouseLocation;
    Collider2D ourBounds;
    public bool isFocus;
    public bool isAlive { get { return alive; } }
    public RobotType Type;
	Animator animator;
	bool facingRight = true;
    //Water player stuff
    public GameObject Water;

    //Breaker player stuff
    public int DestroyCharge = 3;

    public enum RobotType : int
    {
        Waterer = 1,
        Pusher = 2,
        Breaker = 3
    }

    private bool alive;
    

 // Use this for initialization
    void Start () 
    {
        mouseLocation = gameObject.transform.position;
        ourBounds = GetComponent<BoxCollider2D>();
		animator = GetComponent<Animator> ();
        alive = true;
    }
 
 // Update is called once per frame
    void Update () 
    {
        if (isFocus)
        {
            //LookAtMouse();

            LeftClickAction();

            RightClickAction();
        }
    }

    private void LeftClickAction()
    {
//Left Click - Movement
        if (Input.GetMouseButtonUp(0))
        {
            mouseLocation = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        }
        if (new Vector2(gameObject.transform.position.x, gameObject.transform.position.y) != mouseLocation
            && alive)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, mouseLocation,
                2.0f*TimeManager.GetTime(TimeType.Gameplay));

			float dirX = Mathf.Abs(mouseLocation.x - transform.position.x);
			float dirY = Mathf.Abs(mouseLocation.y - transform.position.y);
			if (dirX > dirY) {
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

    private void RightClickAction()
    {
        if (Input.GetMouseButtonUp(1) && Type == RobotType.Waterer)
        {
            GameObject projectileGameObject = Instantiate(Water, transform.position, Quaternion.identity) as GameObject;
            Vector3 mouseLocation2 =
                Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            projectileGameObject.rigidbody2D.AddForce(
                (new Vector2(mouseLocation2.x - transform.position.x, mouseLocation2.y - transform.position.y)).normalized*10,
                ForceMode2D.Impulse);
            projectileGameObject.AddComponent("SquirtTimer");
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
        if (collider.gameObject.tag == "Wall" && DestroyCharge > 0 && Type == RobotType.Breaker)
        {
            Destroy(collider.gameObject);
            DestroyCharge--;
        }

        if (collider.gameObject.tag == "Props")
        {
            if (Type == RobotType.Pusher)
                collider.gameObject.rigidbody2D.isKinematic = false;
            else
                collider.gameObject.rigidbody2D.isKinematic = true;
        }

    }

	void Kill() {
		transform.position = new Vector3 (2.644737f, -0.9473684f, 0);	//spawn zone
		renderer.enabled = false;
	    alive = false;
        Respawn();
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
		facingRight = !facingRight;
		Vector3 nextScale = transform.localScale;
		nextScale.x *= -1f;
		transform.localScale = nextScale;
	}
	
	float Orient(float i) {
		return facingRight ? -i : i;
	}

	void calculateWallScore() {
		
	}
}
