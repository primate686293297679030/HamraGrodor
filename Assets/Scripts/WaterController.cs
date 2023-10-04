using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterController : MonoBehaviour
{
    private Rigidbody2D rb;
    public float rippleForce = 10f;
    private Material waterMaterial;
    private float time = 0.0f;
    public float rippleSpeed = 1.0f;
   private Image image;
    public Vector2 tilingSpeed = new Vector2(1.0f, 0.0f); // Adjust tiling speed in X and Y directions.
    public Vector2 offsetSpeed = new Vector2(0.0f, 0.1f); // Adjust offset speed in X and Y directions.
    private void Start()
    {
        image = GetComponent<Image>();
        rb = GetComponent<Rigidbody2D>();

        waterMaterial = GetComponent<Image>().material;
    }

    private void OnMouseDown()
    {
        // Create a ripple when the mouse is clicked
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 forceDirection = mousePosition - transform.position;
        rb.AddForce(forceDirection.normalized * rippleForce, ForceMode2D.Impulse);
    }
    private void Update()
    {
        // Ensure there is a valid material and shader property to control the ripple.
        if (waterMaterial != null)
        {
            // Increment time based on rippleSpeed to animate the waves.
            time += Time.deltaTime * rippleSpeed;

            // Pass the updated time value to the shader's "_Time" property.
            waterMaterial.SetFloat("_Time", time);



            waterMaterial.SetFloat("_RippleStrength", Mathf.PingPong(Time.time, 3.0f));
            // Adjust tiling and offset properties for more control over the texture.
            Vector2 tiling = new Vector2(Mathf.Sin(Time.time * tilingSpeed.x), Mathf.Cos(Time.time * tilingSpeed.y));
            Vector2 offset = new Vector2(Time.time * offsetSpeed.x, Time.time * offsetSpeed.y);

            waterMaterial.SetTextureScale("_MainTex", tiling);
            waterMaterial.SetTextureOffset("_MainTex", offset);
        }
    }
    private void animateWaves()
    {
        Mathf.Sin(Time.time * 10f);
    }
}

