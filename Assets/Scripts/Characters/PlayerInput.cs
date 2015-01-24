using System;
using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {
    Vector2 mouseLocation;
    Collider2D ourBounds;
    private bool alive;
    public RobotType Type;

    //Water player stuff
    public GameObject Water;

    //Breaker player stuff
    public int DestroyCharge = 3;

    public enum RobotType
    {
        Waterer,
        Pusher,
        Breaker
    }

 // Use this for initialization
    void Start () 
    {
        mouseLocation = gameObject.transform.position;
        ourBounds = GetComponent<BoxCollider2D>();
        alive = true;
    }
 
 // Update is called once per frame
    void Update () 
    {
        if (Input.GetMouseButtonUp (0)) 
        {
            mouseLocation = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
        }
        if (new Vector2(gameObject.transform.position.x, gameObject.transform.position.y) != mouseLocation
            && alive)
        {
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, mouseLocation,
                2.0f*Time.deltaTime);
        }

        if (Input.GetMouseButtonUp(1))
        {
            Debug.Log("Right Click");
            switch (Type)
            {
                case RobotType.Waterer:
                    GameObject projectileGameObject = Instantiate(Water, transform.position, Quaternion.identity) as GameObject;
                    Vector3 mouseLocation2 = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
                    projectileGameObject.rigidbody2D.AddForce((new Vector2(mouseLocation2.x - transform.position.x, mouseLocation2.y - transform.position.y)).normalized, ForceMode2D.Impulse);
                    break;
                case RobotType.Breaker:
                    //Currently Walks into walls and destroys them. Has X amount of charges
                    break;
                case RobotType.Pusher:

                    break;
                default:
                    throw new Exception("RobotType not assigned.");
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.tag == "Wall" && DestroyCharge > 0 && Type == RobotType.Breaker)
        {
            Destroy(collider.gameObject);
            DestroyCharge--;
        }
        Debug.Log(collider.gameObject.tag);
        if (collider.gameObject.tag == "Props" && DestroyCharge > 0 && Type == RobotType.Pusher)
        {
            Destroy(collider.gameObject);
            DestroyCharge--;
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
}
