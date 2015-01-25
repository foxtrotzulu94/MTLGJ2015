using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum TimeType
{
    Engine,
    Gameplay,
}

public class TimeManager : MonoBehaviour
{
    private static float[] m_TimeDilatations = new float[]{1.0f, 1.0f};

    static public void SetTimeDilatation(TimeType type, float dilatation)
    {
        if (type != TimeType.Engine)
        {
            m_TimeDilatations[(int)type] = dilatation;
        }
        else
        {
            // Can't modify Engine time
        }
    }

    static public float GetTimeDilatation(TimeType type)
    {
        return m_TimeDilatations[(int)type];
    }

    static public float GetTime(TimeType type)
    {
        return Time.deltaTime * GetTimeDilatation(type);
    }

    public void Pause()
    {
        TimeManager.SetTimeDilatation(TimeType.Gameplay, 0.0f);
        Camera.main.BroadcastMessage("ShowVignette");
    }

    public void Unpause()
    {
        TimeManager.SetTimeDilatation(TimeType.Gameplay, 1.0f);
		Camera.main.BroadcastMessage("HideVignette");
    }

    float m_SpecialActionTimer = 3.0f;
	public void Update()
	{
        m_SpecialActionTimer -= TimeManager.GetTime(TimeType.Gameplay);
		if(m_SpecialActionTimer <= 0.0f)
		{
			BroadcastMessage("SpecialActionRegistrationEvent");
            SpecialEventManager.Instance.ExecuteSpecialAction(1);
            m_SpecialActionTimer = 3.0f;
		}

		if(Input.GetKeyUp(KeyCode.Space))
		{
            if (TimeManager.GetTimeDilatation(TimeType.Gameplay) <= 0.0f)
            {
				Unpause();
			}
            else
            {
				Pause();
			}
		}
    }
}
