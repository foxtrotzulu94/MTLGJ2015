using UnityEngine;
using System.Collections;

public class LevelBackground : MonoBehaviour {

    public GameObject Background;
    public int SideOfGameWorld = 100;

    void Awake()
    {
        GenerateLevel(SideOfGameWorld, SideOfGameWorld);
    }

	// Use this for initialization
    public void GenerateLevel(int width, int height)
    {
        Random.seed = System.DateTime.UtcNow.Millisecond;

        //Make a Large Room and begin dividing it up
        SpriteRenderer spriteBack = Background.GetComponent<SpriteRenderer>();
        Vector3 origin = new Vector3(-SideOfGameWorld/2, SideOfGameWorld/2, 0);
        //Probably shouldn't take only 1 component, right?
        float startingBounds = spriteBack.bounds.size.x; //For now, we'll assume our sprites are squares.

        int MapSizeX = width;
        int MapSizeY = height;

        Vector3 basePosition = gameObject.transform.position + Vector3.left * (float)MapSizeX / 2.0f + Vector3.down * (float)MapSizeY / 2.0f + new Vector3(0.5f, 0.5f) + Vector3.forward * 0.1f;

        GameObject parent = new GameObject();
        parent.transform.parent = transform;

        GameObject tempFloorTile;
        for (int j = 0; j < MapSizeY; j++)
        {
            for (int i = 0; i < MapSizeX; i++)
            {
                origin.x = i;
                origin.y = j;
                tempFloorTile = (GameObject)GameObject.Instantiate(Background, origin + basePosition, Quaternion.identity);
                tempFloorTile.transform.parent = parent.transform;
            }
        }
    }
}
