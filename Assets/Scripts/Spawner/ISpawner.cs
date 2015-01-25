using UnityEngine;
using System.Collections;

public abstract class ISpawner : MonoBehaviour
{
    public abstract void Spawn(Grid grid);
}
