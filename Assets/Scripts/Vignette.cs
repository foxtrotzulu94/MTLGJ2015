using UnityEngine;
using System.Collections;

public class Vignette : MonoBehaviour
{
	// Update is called once per frame
	void Update ()
    {
        float ratio = Camera.main.aspect;
	    transform.localScale = new Vector3(ratio, 1.0f,1.0f);
	}

    public void OnDrawGizmos()
    {
        Update();
    }
}
