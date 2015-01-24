using System.Collections.Generic;
using UnityEngine;

class SpecialEventManager
{
    private static readonly SpecialEventManager instance = new SpecialEventManager();

    private SpecialEventManager() { }

    public static SpecialEventManager Instance
    {
        get 
        {
            return instance; 
        }
    }

    List<GameObject> m_SpecialEventRegisters = new List<GameObject>();

    public void Register(GameObject registerer)
    {
        m_SpecialEventRegisters.Add(registerer);
    }

    public void ClearRegisterers()
    {
        m_SpecialEventRegisters.Clear();
    }

    public void ExecuteSpecialAction(int numberOfAction)
    {
        for (int i = 0; i < numberOfAction && m_SpecialEventRegisters.Count > 0; ++i)
        {
            int randomIndex = Random.Range(0, m_SpecialEventRegisters.Count);
            m_SpecialEventRegisters[randomIndex].SendMessage("SpecialAction");
        }

        ClearRegisterers();
    }
}
