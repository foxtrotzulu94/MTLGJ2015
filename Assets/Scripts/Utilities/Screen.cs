using UnityEngine;

class ScreenUtility
{
    public static float GetHorizontalFOV()
    {
        float vFOVrad = Camera.main.fieldOfView * Mathf.Deg2Rad;
        float cameraHeightAt1 = Mathf.Tan(vFOVrad *0.5f);
        float hFOVrad = Mathf.Atan(cameraHeightAt1 * Camera.main.aspect) * 2;

        return hFOVrad * Mathf.Rad2Deg;
    }

    public static Vector2 GetScreenMousePosition()
    {
        Vector2 pos = Input.mousePosition;
        pos.y = Screen.height - pos.y;

        return pos;
    }
}
