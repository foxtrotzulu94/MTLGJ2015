using UnityEngine;
using System.Collections;

public class SquirtTimer : MonoBehaviour {

    public float lifeTime = 0.5f;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        lifeTime -= TimeManager.GetTime(TimeType.Gameplay);
        Debug.Log(lifeTime);
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
