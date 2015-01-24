using UnityEngine;
using System.Collections;

public class fire : MonoBehaviour {
	static int tot = 0;
	float fireSpawnRate = 3f;
	float fireSpawnTimer;
	int fireCreated = 0;
	//public GameObject fireObject;
	Vector3 initialFirePositionRight;

	// Use this for initialization
	void Start () {
		fireSpawnTimer = fireSpawnRate;
		initialFirePositionRight = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		fireSpawnTimer -= Time.deltaTime;
		if (fireSpawnTimer <= 0 && fireCreated < 3 && tot < 50) {
			//GameObject fireNextRight = Instantiate(fireObject, initialFirePositionRight + new Vector3(0.2f, 0f, 0f), Quaternion.identity) as GameObject;
			//GameObject fireNextLeft = Instantiate(fireObject, initialFirePositionLeft + new Vector3(-0.2f, 0f, 0f), Quaternion.identity) as GameObject;
			//GameObject fireNextUp = Instantiate(fireObject, initialFirePositionUp + new Vector3(0f, 0.2f, 0f), Quaternion.identity) as GameObject;
			//GameObject fireNextDown = Instantiate(fireObject, initialFirePositionDown + new Vector3(0f, -0.2f, 0f), Quaternion.identity) as GameObject;
			
			var x = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward) * Vector3.right;
			Debug.Log (x.normalized);
			GameObject fireNextRight = Instantiate(this, initialFirePositionRight + new Vector3(0.3f * x.x,0.3f * x.y,0.3f * x.z), Quaternion.identity) as GameObject;
			//
			//			GameObject fireNextRight = Instantiate(fireObject, initialFirePositionRight + new Vector3(Random.Range (0f, 0.2f), Random.Range (-0.2f, 0.2f), 0f), Quaternion.identity) as GameObject;
			//			GameObject fireNextLeft = Instantiate(fireObject, initialFirePositionLeft + new Vector3(Random.Range (-0.2f, 0f), Random.Range (-0.2f, 0.2f), 0f), Quaternion.identity) as GameObject;
			//			GameObject fireNextUp = Instantiate(fireObject, initialFirePositionUp + new Vector3(Random.Range (-0.2f, -0.2f), Random.Range (0f, 0.2f), 0f), Quaternion.identity) as GameObject;
			//			GameObject fireNextDown = Instantiate(fireObject, initialFirePositionDown + new Vector3(Random.Range (-0.2f, -0.2f), Random.Range (-0.2f, 0f), 0f), Quaternion.identity) as GameObject;
			initialFirePositionRight = fireNextRight.transform.position;
			//			initialFirePositionLeft = fireNextLeft.transform.position;
			//			initialFirePositionUp = fireNextUp.transform.position;
			//			initialFirePositionDown = fireNextDown.transform.position;
			fireSpawnTimer = fireSpawnRate;
			fireCreated++;
			tot++;
	}
}
}
