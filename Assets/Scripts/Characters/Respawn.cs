using UnityEngine;
using System.Collections;

public class Respawn : MonoBehaviour {

    public float lifeTime = 2f;
    private GameObject player;

	// Use this for initialization
	void Start ()
	{
	    player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
    private void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            player.SendMessage("Revived");
            Destroy(this);
        }
    }
}
