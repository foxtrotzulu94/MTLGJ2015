using UnityEngine;
using System.Collections;

public class Vignette : MonoBehaviour
{
    public bool Enabled = true;
    public float FadingRatio = 0.1f;

    void Start()
    {
        renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, Enabled ? 1.0f : 0.0f);
    }

    private void Resize()
    {
        float ratio = Camera.main.aspect;
        float scaleRatio = Camera.main.orthographicSize / 5.0f;
        transform.localScale = new Vector3(ratio * scaleRatio, 1.0f, 1.0f * scaleRatio);
    }

	// Update is called once per frame
	void Update ()
    {
        Resize();


        float step = Mathf.Min(FadingRatio * TimeManager.GetTime(TimeType.Engine), 1.0f);
        float alpha = Mathf.Lerp(renderer.material.color.a, Enabled ? 1.0f : 0.0f, step);
        renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, alpha);
	}

    public void OnDrawGizmos()
    {
        Resize();

        renderer.sharedMaterial.color = new Color(renderer.sharedMaterial.color.r, renderer.sharedMaterial.color.g, renderer.sharedMaterial.color.b, Enabled ? 1.0f : 0.0f);
        renderer.sharedMaterial = renderer.sharedMaterial;
    }

	public void ShowVignette()
    {
        Enabled = true;
    }

    public void HideVignette()
    {
        Enabled = false;
    }
}
