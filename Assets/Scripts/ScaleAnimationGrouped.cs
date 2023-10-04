using UnityEngine;

public class ScaleAnimationGrouped : MonoBehaviour
{
    public float amplitudeY = 0.2f; // The amplitude of the sinusoidal scale animation.
    public float frequencyY = 0.05f; // The frequency of the sinusoidal scale animation.
    public float amplitudeX = 0.2f; // The amplitude of the sinusoidal scale animation.
    public float frequencyX = 0.05f; // The frequency of the sinusoidal scale animation.

    private Vector3[] initialScale;

    private float startTime;

    void Start()
    {
        startTime = Time.time;
        initialScale = new Vector3[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            initialScale[i] = transform.GetChild(i).localScale;


        }

    }

    void Update()
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            // Calculate the time elapsed since the animation started.
            float timeElapsed = Time.time - startTime;

            // Calculate the new scale using a sinusoidal pattern.
            float newScale = initialScale[i].y + amplitudeY * Mathf.Sin(2 * Mathf.PI * frequencyY * timeElapsed);
            float newScalex = initialScale[i].x + amplitudeX * Mathf.Sin(2 * Mathf.PI * frequencyX * timeElapsed);
            newScalex = Mathf.Max(newScalex, initialScale[i].x);

            // Apply the new scale to the GameObject.
            Vector3 newScaleVector = new Vector3(newScalex, newScale, initialScale[i].z);
            //transform.localScale = newScaleVector;

            transform.GetChild(i).localScale = newScaleVector;
        }
    }
}

