using UnityEngine;

public class ScaleAnimation : MonoBehaviour
{
    public float amplitudeY = 0.2f; // The amplitude of the sinusoidal scale animation.
    public float frequencyY = 0.0f; // The frequency of the sinusoidal scale animation.
    public float amplitudeX = 0.2f; // The amplitude of the sinusoidal scale animation.
    public float frequencyX = 0.05f; // The frequency of the sinusoidal scale animation.

    private Vector3 initialScale;
 
    private float startTime;

    void Start()
    {
        initialScale = transform.localScale;
        startTime = Time.time;
    }

    void Update()
    {
        // Calculate the time elapsed since the animation started.
        float timeElapsed = Time.time - startTime;

        // Calculate the new scale using a sinusoidal pattern.
        float newScale = initialScale.y + amplitudeY * Mathf.Sin(2 * Mathf.PI * frequencyY * timeElapsed);
        float newScalex = initialScale.x + amplitudeX * Mathf.Sin(2 * Mathf.PI * frequencyX* timeElapsed);
        newScalex = Mathf.Max(newScalex, initialScale.x);

        // Apply the new scale to the GameObject.
        Vector3 newScaleVector = new Vector3(newScalex, newScale, initialScale.z);
        transform.localScale = newScaleVector;
    }
}

