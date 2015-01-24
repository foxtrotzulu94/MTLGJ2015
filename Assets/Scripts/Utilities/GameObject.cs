using System.Collections.Generic;
using UnityEngine;

class GameObjectUtility
{
    public static List<GameObject> GetChildrensByTag(GameObject parent, string tag)
    {
        List<GameObject> gameObjects = new List<GameObject> ();
        for(int i = 0; i < parent.transform.childCount; ++i)
        {
            if(parent.transform.GetChild(i).tag == tag)
            {
                gameObjects.Add (parent.transform.GetChild(i).gameObject);
            }
        }
        
        return gameObjects;
    }

    public static GameObject GetChildrenByTag(GameObject parent, string tag)
    {
        for(int i = 0; i < parent.transform.childCount; ++i)
        {
            if(parent.transform.GetChild(i).tag == tag)
            {
                return parent.transform.GetChild(i).gameObject;
            }
        }
        
        return null;
    }

    public static T GetOrAddComponent<T>(GameObject gameObject) where T : Component
    {
        T component = gameObject.GetComponent<T>();
        if (component == null)
        {
            component = gameObject.AddComponent<T>();
        }

        return component;
    }
}
