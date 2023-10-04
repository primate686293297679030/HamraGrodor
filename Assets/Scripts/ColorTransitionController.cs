using UnityEngine;

public class ColorTransitionController : MonoBehaviour
{
    public Material material; // Assign your material containing the ColorTransition shader in the Inspector

    public float transitionDuration = 2f; // Set the duration of the color transition in seconds

    void Update()
    {
        float time = Mathf.Repeat(Time.time, transitionDuration) / transitionDuration;
        float blendValue = Mathf.SmoothStep(0f, 1f, time);
        material.SetFloat("_Blend", blendValue);
    }
}
