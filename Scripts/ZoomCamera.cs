
using UnityEngine;

public class ZoomCamera : MonoBehaviour
{
    Vector3 touchStart;

    float zoomOutMin = 3.7f;
    private float zoomOutMax = 5;float luu = 5;
    public bool duoczoom = true;

    private float zoomSpeed = 4f;
    private float targetOrtho = 5;
    private void OnEnable()
    {
        Camera.main.orthographicSize = luu;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        // Check for mouse button down
        if (Input.GetMouseButtonDown(0))
        {
            touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        // Check if zooming is enabled
        if (duoczoom)
        {
            // Pinch to zoom for touch devices
            if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

                float difference = currentMagnitude - prevMagnitude;

                targetOrtho -= difference * 0.1f;
                targetOrtho = Mathf.Clamp(targetOrtho, zoomOutMin, zoomOutMax);
            }

            // Scroll wheel zoom for PC
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            targetOrtho -= scroll * zoomSpeed;
            targetOrtho = Mathf.Clamp(targetOrtho, zoomOutMin, zoomOutMax);

            // Smooth zoom transition
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetOrtho, Time.deltaTime * zoomSpeed);
        }

    }
}
