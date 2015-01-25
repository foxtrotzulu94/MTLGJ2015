using UnityEngine;
using System.Collections;

public class fireSpawner : MonoBehaviour {

	Vector2 position;
	public GameObject fireObject;

	// Use this for initialization
	void Start () {

		position = new Vector2 (Random.Range (-2f, 2f), Random.Range (-1.5f, 1.5f));
	    Instantiate(fireObject, position, Quaternion.identity);

	}
	
	// Update is called once per frame
	void Update () {

		}
		
	}

