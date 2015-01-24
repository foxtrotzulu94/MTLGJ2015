using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {
    Vector2 mouseLocation;
    Collider2D ourBounds;
    private bool alive;

 // Use this for initialization
    void Start () 
    {
        mouseLocation = this.gameObject.transform.position;
        ourBounds = this.GetComponent<BoxCollider2D>();
        alive = true;
    }
 
 // Update is called once per frame
    void Update () 
    {
      if (Input.GetMouseButtonUp (0)) 
      {
        mouseLocation = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
      }
      if (new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y) != mouseLocation 
          && alive)
        this.gameObject.transform.position = Vector3.MoveTowards (this.gameObject.transform.position, mouseLocation, 2.0f * Time.deltaTime);
 
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
}
