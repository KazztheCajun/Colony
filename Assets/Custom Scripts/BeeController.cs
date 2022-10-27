using System;
using System.Collections.Generic;
using UnityEngine;

public class BeeController : MonoBehaviour
{

    // Public vaiables
    public Transform selectionArea;

    // Private variables
    private Camera cam;
    private Vector3 startPosition;
    private List<Bee> selectedBees;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        selectedBees = new List<Bee>();
        selectionArea.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) // Left mouse button pressed
        {
            startPosition = cam.ScreenToWorldPoint(Input.mousePosition); // store the mouse pos on first click
            selectionArea.gameObject.SetActive(true); // show selection rectangle
        }

        if(Input.GetMouseButton(0)) // Left mouse button held down
        {
            Vector3 currentMouse = cam.ScreenToWorldPoint(Input.mousePosition); // store current mouse pos
            // calculate the lower right point of the selection area
            Vector3 lowerLeft = new Vector3(Mathf.Min(startPosition.x, currentMouse.x), Mathf.Min(startPosition.y, currentMouse.y), 1);
            // calculate the upper right point of the selection area
            Vector3 upperRight = new Vector3(Mathf.Max(startPosition.x, currentMouse.x), Mathf.Max(startPosition.y, currentMouse.y), 1);
            selectionArea.position = lowerLeft; // set the position of the selection area
            selectionArea.localScale = upperRight - lowerLeft; // scale the selection area
        }

        if(Input.GetMouseButtonUp(0)) // Left mouse button released
        {
            selectionArea.gameObject.SetActive(false); //  hide selection area
            //Debug.Log($"Pressed: {startPosition} | Released: {cam.ScreenToWorldPoint(Input.mousePosition)}")
            Collider2D[] colliderArray = Physics2D.OverlapAreaAll(startPosition, cam.ScreenToWorldPoint(Input.mousePosition), 1); // find all the colliders inside the selection area
            //Debug.Log("#######");
            foreach(Bee b in selectedBees) // deselect any bees
            {
                b.deselectBee();
            }
            selectedBees.Clear();
            foreach(Collider2D c in colliderArray)
            {
                //Debug.Log(c);
                Bee b = c.GetComponent<Bee>();
                if (b != null)
                {
                    selectedBees.Add(b);
                    b.selectBee();
                }
            }
            //Debug.Log(selectedBees.Count);
        }

        if (Input.GetMouseButtonDown(1)) // Right mouse button pressed
        {
            Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);
            List<Vector3> list = GetPositionsAround(new Vector3(pos.x, pos.y, 1), new float[] {1f, 2f, 3f}, new int[] {5, 10, 20});
            int index = 0;
            shuffle(selectedBees);
            foreach(Bee b in selectedBees)
            {
                b.target = list[index];
                index = (index + 1) % list.Count;
                b.state = Bee.BeeState.Harvest;
            }
        }
        
    }

    private List<Vector3> GetPositionsAround(Vector3 start, float[] ringDistArray, int[] numArray)
    {
        List<Vector3> l = new List<Vector3>();
        l.Add(start);
        for (int i = 0; i < ringDistArray.Length; i++)
        {
            l.AddRange(GetPositions(start, ringDistArray[i], numArray[i]));
        }
        return l;
    }

    private List<Vector3> GetPositions(Vector3 start, float dist, int num)
    {
        List<Vector3> l = new List<Vector3>();
        for(int i = 0; i < num; i++)
        {
            float angle = i * (360f / num);
            Vector3 dir = ApplyRotationToVector(new Vector3(1, 0), angle);
            Vector3 pos = start + dir * dist;
            l.Add(pos);
        }
        return l;
    }

    private Vector3 ApplyRotationToVector(Vector3 vec, float angle)
    {
        return Quaternion.Euler(0, 0, angle) * vec;
    }

    // Adpated from https://stackoverflow.com/questions/273313/randomize-a-listt
    private static void shuffle(List<Bee> list)  
    {  
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = (int) Math.Floor((double) UnityEngine.Random.Range(0, n + 1));  
            Bee value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }  
    }
}
