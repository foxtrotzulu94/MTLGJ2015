using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {

    public Color BackgroundColor = Color.white;

    // Use this for initialization
    void Start()
    {
        Camera.main.backgroundColor = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            Debug.Log(Application.loadedLevelName);
            Application.LoadLevel("Menu");
        }
    }
}
