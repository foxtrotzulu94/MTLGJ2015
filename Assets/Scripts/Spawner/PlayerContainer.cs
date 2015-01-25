using System.Collections.Generic;
using UnityEngine;

class PlayerContainer
{
    public List<GameObject> PlayerObjects = new List<GameObject>();

    private static readonly PlayerContainer instance = new PlayerContainer();

    private PlayerContainer() { }

    public static PlayerContainer Instance
    {
        get 
        {
            return instance; 
        }
    }

    public bool InitialSpawning = true;

    public int level = 0;
}
