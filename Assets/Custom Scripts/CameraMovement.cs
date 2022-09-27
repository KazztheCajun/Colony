using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public Camera cam;
    public float zoomStep, minSize, maxSize, speed, edge;
    public GameManager manager;

    private Vector3 dragOrigin;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        PanCamera();

        float newSize = cam.orthographicSize - (Input.mouseScrollDelta.y * zoomStep);
        cam.orthographicSize = Mathf.Clamp(newSize, minSize, maxSize);
    }

    private void PanCamera()
    {
        Vector3 move = Vector3.zero;
        Transform t = cam.GetComponent<Transform>(); 
        move.x = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        move.y = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        if(manager.mousePan)
        {
            if(Input.mousePosition.x > Screen.width - edge)
            {
                move.x += speed * Time.deltaTime;
            }
            if(Input.mousePosition.x < edge)
            {
                move.x -= speed * Time.deltaTime;
            }
            if(Input.mousePosition.y > Screen.height - edge)
            {
                move.y += speed * Time.deltaTime;
            }
            if(Input.mousePosition.y < edge)
            {
                move.y -= speed * Time.deltaTime;
            }
        }
        
        t.position += move;
    }

    public void ZoomIn()
    {
        float newSize = cam.orthographicSize - zoomStep;
        //Debug.Log("Zoom In Clicked!");
        cam.orthographicSize = Mathf.Clamp(newSize, minSize, maxSize);
    }

    public void ZoomOut()
    {
        float newSize = cam.orthographicSize + zoomStep;
        //Debug.Log("Zoom Out Clicked");
        cam.orthographicSize = Mathf.Clamp(newSize, minSize, maxSize);
    }
}
