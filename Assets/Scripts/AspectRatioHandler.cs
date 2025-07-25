using UnityEngine;

public class AspectRatioHandler : MonoBehaviour
{
    Camera _camera;
  
    private void Start()
    {
        _camera = GetComponent<Camera>();
        ApplyAspectRatio();
    }

    private void Update()
    {
        // Apply the aspect ratio at the start and check for changes if needed
        ApplyAspectRatio();

    }

    private void ApplyAspectRatio()
    {
        // Calculate the desired aspect ratio (16:9)
        float targetWidth = 16f;
        float targetHeight = 9f;
        float targetAspect = targetWidth / targetHeight;

        // Calculate the current aspect ratio
        float currentAspect = (float)Screen.width / Screen.height;

        float rectX = 0f;
        float rectY = 0f;
        float rectWidth = 1f;
        float rectHeight = 1f;

        // Adjust the aspect ratio if necessary
        if (currentAspect > targetAspect)
        {
            // Screen is wider, add pillarboxing
            float widthScale = targetAspect / currentAspect;
            rectWidth = widthScale;
            rectX = (1f - widthScale) / 2f;
        }
        else
        {
            // Screen is taller, add letterboxing
            float heightScale = currentAspect / targetAspect;
            rectHeight = heightScale;
            rectY = (1f - heightScale) / 2f;
        }

        // Apply the calculated aspect ratio to the camera's rect
        Rect rect = new Rect(rectX, rectY, rectWidth, rectHeight);
        _camera.rect = rect;
    }

}


