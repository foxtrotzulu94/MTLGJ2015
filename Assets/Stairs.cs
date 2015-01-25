using UnityEngine;
using System.Collections;

public class Stairs : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D someObject)
    {
        RobotInput aRobot = someObject.GetComponentInParent<RobotInput>();
        if (aRobot != null)
        {
            //This thing must be a robot
            aRobot.SendToNextLevel();
            Debug.Log("Sending to next level");
        }
    }
}
