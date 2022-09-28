using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hive : MonoBehaviour
{
    public int maxHoney;
    public Slider honeyBar;


    // Start is called before the first frame update
    void Start()
    {
        honeyBar.maxValue = maxHoney;
        honeyBar.value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
