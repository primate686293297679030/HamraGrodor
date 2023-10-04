using UnityEngine;

public class WaveAnimation : MonoBehaviour
{
    public float waveFrequency = 1.0f; // Controls the frequency of the wave.
    public float waveAmplitudeY = 0.1f; // Controls the amplitude of the wave.
    public float waveAmplitudeX = 0.1f; // Controls the amplitude of the wave.

    public float waveSpeedY = 1.0f; // Controls the speed of the wave.
    public float waveSpeedX = 1.0f; // Controls the speed of the wave.
    

    private Vector3[] initialPositions;

    void Start()
    {
        // Store the initial positions of the segmented sprites.
        initialPositions = new Vector3[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            initialPositions[i] = transform.GetChild(i).position;
        }
    }

    void Update()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            // Apply a wave-like motion to each segmented sprite.
            Vector3 newPosition = initialPositions[i];
            newPosition.y += Mathf.Sin(Time.time * waveSpeedY + i * waveFrequency) * waveAmplitudeY;
            newPosition.x += Mathf.Cos(Time.time * waveSpeedX + i * waveFrequency) * waveAmplitudeX;
            transform.GetChild(i).position = newPosition;
        }
    }
}

