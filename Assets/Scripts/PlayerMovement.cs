using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public float CameraSpeed;
    public float CameraAcceleration;

    private Vector2 inputVectors;
    private float xTime;
    private float yTime;

	// Use this for initialization
	void Start () {
	    //Nothing to do here, right??
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.LoadLevel("Menu");
        }


        inputVectors.x = Input.GetAxisRaw("Horizontal") * CameraSpeed * Time.deltaTime;
        inputVectors.y = Input.GetAxisRaw("Vertical") * CameraSpeed * Time.deltaTime;

        //Want to have Max and Min zoom Size for Camera to avoid Aliasing
        float scrollWheel = Input.GetAxisRaw("Mouse ScrollWheel");
        if (scrollWheel > 0)
        {
            Camera.main.orthographicSize -= 1;
        }
        else if (scrollWheel < 0)
        {
            Camera.main.orthographicSize += 1;
        }
        //Debug.Log(Input.GetAxis("Horizontal") + " " + Input.GetAxis("Vertical"));
        transform.Translate(inputVectors.x , inputVectors.y , 0);
        
	}
}
