using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    public float CameraSpeed;
    public float CameraAcceleration;

    private RobotInput[] RobotArray;
    private RobotInput.RobotType RobotInFocus;
    private int inputNumber;
    private int previousInput;
    private Vector2 inputVectors;
    private float xTime;
    private float yTime;

	// Use this for initialization
	void Start () {
        //Need to Retrieve Robots from Spawn and not sent out from prefab or we are doomed :/
        RobotArray = new RobotInput[0];
	    //Nothing else to do here, right??
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Application.LoadLevel("Menu");
        }

        if (RobotArray.Length == 0)
        {
            RetrieveRobotControllers();
        }
        else
        {
            HandleRobots();
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

    private void RetrieveRobotControllers()
    {
        RobotArray = (RobotInput[])Object.FindObjectsOfType<RobotInput>(); //Hopefully these are the ones we want, right?
        Debug.Log(RobotArray);
        Debug.Log("Initializing Player Movement...");
        foreach (RobotInput a in RobotArray)
        {
            Debug.Log(a.name);
        }
        inputNumber = 0;
        RobotArray[inputNumber].isFocus = true;
        RobotInFocus = RobotArray[inputNumber].Type;
        Debug.Log(RobotInFocus);
    }

    private void HandleRobots()
    {
        previousInput = inputNumber;
        //Get The Inputs
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            inputNumber = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            inputNumber = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            inputNumber = 2;
        }

        if (inputNumber == previousInput)
        {
            //Nothing to do here then, right?
            return;
        }

        for (int i = 0; i < RobotArray.Length; i++)
        {
            if (i == inputNumber)
            {
                RobotArray[inputNumber].isFocus = true;
                RobotInFocus = RobotArray[inputNumber].Type;
            }
            else
            {
                RobotArray[i].isFocus = false;
            }
        }
    }

}
