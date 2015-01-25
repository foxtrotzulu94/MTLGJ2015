using UnityEngine;
using System.Collections;

public class score : MonoBehaviour {

	int totalScore;
    public int WallMultiplier;
	public TextMesh text;

	// Use this for initialization
	void Start () {
		totalScore = 0;
	}
	
	// Update is called once per frame
	void Update () {
		text.text = "Score: " + totalScore;
	}

	void Increment(int i) {
		totalScore += i;
	}

    public void CountRemainingWalls()
    {
        int remainingWalls = GameObject.FindObjectsOfType<Flammable>().Length;
        Increment(WallMultiplier * remainingWalls);
    }
}
