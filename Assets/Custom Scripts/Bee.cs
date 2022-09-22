using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : MonoBehaviour
{
    public int speed;
    public int capacity;
    public int nectar;
    public int energy;

    public Transform target;


    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            transform.RotateAround(target.transform.position, Vector3.up, speed * Time.deltaTime);
        }
    }
}
