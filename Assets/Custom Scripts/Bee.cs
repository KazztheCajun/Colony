using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bee : MonoBehaviour
{
    // public variables
    public int speed;
    public int capacity;
    public int maxEnergy;
    private bool isSelected;

    // reference variables
    public Slider energyBar;
    public Slider nectarBar;
    public GameObject infoPanel;
    private SpriteRenderer render;

    // private variables
    [HideInInspector]
    public Transform target;
    [HideInInspector]
    public string id;
    private int energy;
    private int nectar;


    // Start is called before the first frame update
    void Start()
    {
        isSelected = false;
        render = GetComponent<SpriteRenderer>();
        energy = maxEnergy;
        nectar = 0;
        energyBar.maxValue = maxEnergy;
        energyBar.value = maxEnergy;
        nectarBar.maxValue = capacity;
        nectarBar.value = nectar;
        infoPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            transform.RotateAround(target.transform.position, Vector3.back, speed * Time.deltaTime);
        }

        energyBar.value = energy;
        nectarBar.value = nectar;
    }

    void OnMouseDown()
    {
        clickBee();
    }

    public void clickBee()
    {
        if(isSelected)
        {
            deselectBee();
        }
        else
        {
            selectBee();
        }
        isSelected = !isSelected;
    }

    private void selectBee()
    {
        //Debug.Log($"{id} has been selected.");
        infoPanel.SetActive(true);
        render.color = Color.yellow;
    }

    private void deselectBee()
    {
        //Debug.Log($"{id} has been deselected.");
        infoPanel.SetActive(false);
        render.color = Color.black;
    }
}
