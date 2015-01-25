using UnityEngine;
using System.Collections;

public class TimeAffected : MonoBehaviour
{

    public void Update()
    {
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.speed = TimeManager.GetTimeDilatation(TimeType.Gameplay);
        }
    }
}
