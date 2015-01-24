using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	    Camera.main.backgroundColor = Color.white;
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.anyKey)
	    {
	        Application.LoadLevel("GridTest");
	    }
	}
}
