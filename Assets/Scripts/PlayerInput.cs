using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {
    Vector2 mouseLocation;
    Collider2D ourBounds;

 // Use this for initialization
    void Awake () 
    {
        mouseLocation = this.gameObject.transform.position;
        ourBounds = this.GetComponent<BoxCollider2D>();
    }
 
 // Update is called once per frame
    void Update () 
    {
      if (Input.GetMouseButtonUp (0)) 
      {
        mouseLocation = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
      }
      if (new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.y) != mouseLocation)
        this.gameObject.transform.position = Vector3.MoveTowards (this.gameObject.transform.position, mouseLocation, 2.0f * Time.deltaTime);
        
        //if(ourBounds.)

 
    }
}
