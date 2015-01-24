using UnityEngine;
using System.Collections;

public class robotMovement : MonoBehaviour {

	GameObject player;
	Vector2 mouseLocation;

	// Use this for initialization
	void Awake () {
		player = GameObject.FindGameObjectWithTag("Player");
		mouseLocation = player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonUp (0)) {
			mouseLocation = camera.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
		}
		if (new Vector2(player.transform.position.x, player.transform.position.y) != mouseLocation)
			player.transform.position = Vector3.MoveTowards (player.transform.position, mouseLocation, 20.0f * Time.deltaTime);


	
	}
}
