using UnityEngine;
using System.Collections;

public class SquirtTimer : MonoBehaviour {

    public float lifeTime = 0.5f;
    public Vector3 direction;
    public float Speed = 10.0f;

    // Update is called once per frame
    private void Update()
    {
        transform.position += direction * Speed * TimeManager.GetTime(TimeType.Gameplay);

        lifeTime -= TimeManager.GetTime(TimeType.Gameplay);
        Debug.Log(lifeTime);
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }
}
