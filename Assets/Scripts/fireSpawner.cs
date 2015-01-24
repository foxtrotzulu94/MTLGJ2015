using UnityEngine;
using System.Collections;

public class fireSpawner : MonoBehaviour {

	Vector2 position;
	public GameObject fireObject;


	Vector3 initialFirePositionRight;
	Vector3 initialFirePositionLeft;
	Vector3 initialFirePositionUp;
	Vector3 initialFirePositionDown;

	GameObject fire1;
	// Use this for initialization
	void Start () {

			position = new Vector2 (Random.Range (-2f, 2f), Random.Range (-1.5f, 1.5f));
			fire1 = Instantiate(fireObject, position, Quaternion.identity) as GameObject;
			initialFirePositionRight = fire1.transform.position;
			initialFirePositionLeft = fire1.transform.position;
			initialFirePositionUp = fire1.transform.position;
			initialFirePositionDown = fire1.transform.position;
	}
	
	// Update is called once per frame
	void Update () {

		}
		
	}

