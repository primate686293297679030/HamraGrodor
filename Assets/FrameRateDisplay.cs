using UnityEngine;

public class FrameRateDisplay : MonoBehaviour
{
    float deltaTime = 0.0f;
    GUIStyle style;
    Texture2D backgroundTexture;

    void Start()
    {
        style = new GUIStyle();
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = Screen.height * 2 / 50;
        style.normal.textColor = Color.white;

        // Create a simple 1x1 texture for clearing
        backgroundTexture = new Texture2D(1, 1);
        backgroundTexture.SetPixel(0, 0, Color.black);
        backgroundTexture.Apply();
    }

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        // Draw the black texture to clear the area
        GUI.DrawTexture(new Rect(10, 10, 225, 75), backgroundTexture);

        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.} FPS", fps);
        GUI.Label(new Rect(10, 10, 200, 50), text, style);
    }
}
