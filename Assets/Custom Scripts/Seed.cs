using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{
    public GameObject parentPlant;
    public int germinationTime;
    private int spawnTime; 
    // Start is called before the first frame update
    void Start()
    {
        spawnTime = Time.frameCount;
        germinationTime = Random.Range(3600, 14400);
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.frameCount - germinationTime >= germinationTime)
        {
            GameObject t = Instantiate(parentPlant, this.transform.position, Quaternion.identity);
            t.transform.Rotate(0, 0, Random.Range(0, 360));
            Destroy(this.transform.gameObject);
        }
    }
}
