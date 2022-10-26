using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{
    public GameObject parentPlant;
    [Range(0, 300f)]
    public int germinationTime;
    private float spawnTime; 
    // Start is called before the first frame update
    void Start()
    {
        spawnTime = 0;
        germinationTime = Random.Range(10, 20);
    }

    // Update is called once per frame
    void Update()
    {
        spawnTime += Time.deltaTime;
        if(spawnTime >= germinationTime)
        {
            GameObject t = Instantiate(parentPlant, this.transform.position, Quaternion.identity);
            t.transform.Rotate(0, 0, Random.Range(0, 360));
            Destroy(this.transform.gameObject);
        }
    }
}
