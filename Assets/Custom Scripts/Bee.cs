using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bee : MonoBehaviour
{
    // public variables
    public int flySpeed;
    public int waitSpeed;
    public int capacity;
    public int maxEnergy;
    public float waitRadius;
    public Slider energyBar;
    public Slider nectarBar;
    public GameObject infoPanel;
    public BeeState state;
    
    // private/hidden variables
    private SpriteRenderer render;
    
    private float rotationAngle;
    [HideInInspector]
    public Vector3 target;
    [HideInInspector]
    public string id;
    [HideInInspector]
    public int energy;
    [HideInInspector]
    public int nectar;
    
    // enumerated values
    public enum BeeState
    {
        Wait,
        Harvest
    }

    // Start is called before the first frame update
    void Start()
    {
        state = BeeState.Wait;
        render = GetComponent<SpriteRenderer>();
        energy = maxEnergy;
        nectar = 0;
        energyBar.maxValue = maxEnergy;
        energyBar.value = maxEnergy;
        nectarBar.maxValue = capacity;
        nectarBar.value = nectar;
        target = transform.position;
        infoPanel.SetActive(false);
        rotationAngle = 0;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case BeeState.Wait:
                fly();
                break;
            case BeeState.Harvest:
                fly();
                break;
        }

        

        energyBar.value = energy;
        nectarBar.value = nectar;
    }

    public void fly()
    {
        if (Vector3.Distance(target, transform.position) > 2f) // if target is farther away than .1 units, move to it
        {
            flyToTarget();
        }
        else
        {
            flyAroundTarget();
        }
    }

    public void flyToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, flySpeed * Time.deltaTime);
    }

    public void flyAroundTarget()
    {
        Vector3 offset = new Vector3(Mathf.Sin(rotationAngle) * waitRadius, Mathf.Cos(rotationAngle) * waitRadius, 0);
        transform.position = Vector3.MoveTowards(transform.position, target + offset, Time.deltaTime * waitSpeed);
        rotationAngle += Time.deltaTime * waitSpeed;
    }

    public void selectBee()
    {
        //Debug.Log($"{id} has been selected.");
        infoPanel.SetActive(true);
        render.color = Color.yellow;
    }

    public void deselectBee()
    {
        //Debug.Log($"{id} has been deselected.");
        infoPanel.SetActive(false);
        render.color = Color.black;
    }
}
