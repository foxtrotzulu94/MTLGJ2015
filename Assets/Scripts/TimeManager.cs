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
}
