using UnityEngine;
using System.Collections;

public class score : MonoBehaviour {

    public int WallMultiplier = 2;
    public int CivilianMultiplier = 5;
	public TextMesh text;

	// Use this for initialization
	void Start () {
		text.transform.position = new Vector3 (-Camera.main.aspect * Camera.main.orthographicSize + 1.0f, Camera.main.orthographicSize - 1.0f, text.transform.position.z);
	}
	
	// Update is called once per frame
	void Update ()
	{
        text.transform.position = new Vector3(-Camera.main.aspect * Camera.main.orthographicSize + 1.0f, Camera.main.orthographicSize - 1.0f, text.transform.position.z);
		text.text = "Score: " + PlayerContainer.Instance.score;

	}

	public void Increment(int i)
	{
		PlayerContainer.Instance.score += i;
	}

    public void CountRemainingWalls()
    {
        int remainingWalls = GameObject.FindObjectsOfType<Flammable>().Length;
        Increment(WallMultiplier * remainingWalls);
    }

    public void CountRescuedCivilians()
    {
        RobotInput[] allRobots = (RobotInput[])Object.FindObjectsOfType<RobotInput>();
        int totalCount=0;
        for (int i = 0; i < allRobots.Length; i++)
        {
            totalCount+=allRobots[i].civiliansRescued;
        }
        Increment(totalCount);
    }
}
