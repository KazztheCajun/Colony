using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public Camera cam;
    public float zoomStep, minSize, maxSize;

    private Vector3 dragOrigin;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PanCamera();
    }

    private void PanCamera()
    {
        // save pos of first mouse click
        if(Input.GetMouseButtonDown(0))
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        }
        // calc dist between initial pos and new position while it is held down
        if (Input.GetMouseButton(0))
        {
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);

            Debug.Log($"Origin: {dragOrigin} | New Spot: {cam.ScreenToWorldPoint(Input.mousePosition)} | Difference: {difference}");
            // move the camera that dist
            cam.transform.position += difference;

        }
        
    }

    public void ZoomIn()
    {
        float newSize = cam.orthographicSize - zoomStep;
        Debug.Log("Zoom In Clicked!");
        cam.orthographicSize = Mathf.Clamp(newSize, minSize, maxSize);
    }

    public void ZoomOut()
    {
        float newSize = cam.orthographicSize + zoomStep;
        Debug.Log("Zoom Out Clicked");
        cam.orthographicSize = Mathf.Clamp(newSize, minSize, maxSize);
    }
}
