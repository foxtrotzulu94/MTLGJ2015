using UnityEngine;
using System.Collections;

public class fire : MonoBehaviour {
	public const int MAX_FIRE = 50;
	static int totalFire = 0;
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
		if (fireSpawnTimer <= 0 && fireCreated < 3 && totalFire < MAX_FIRE) {

			var angle = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward) * Vector3.right;
			Instantiate(this, initialFirePositionRight + new Vector3(0.3f * angle.x,0.3f * angle.y,0.3f * angle.z), Quaternion.identity);

			fireSpawnTimer = fireSpawnRate;
			fireCreated++;
			totalFire++;
	}
}
}
