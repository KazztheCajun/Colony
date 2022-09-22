using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // prefab references
    public GameObject hive;
    public GameObject bee;
    public List<GameObject> flowers;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(hive);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
