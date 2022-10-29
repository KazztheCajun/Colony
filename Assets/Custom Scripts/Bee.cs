using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bee : MonoBehaviour
{
    // public variables
    [Range(0, 500)]
    public int flySpeed;
    [Range(0, 500)]
    public int waitSpeed;
    [Range(0, 10f)]
    public float waitTime;
    [Range(0, 200)]
    public int drinkSpeed;
    [Range(0, 500f)]
    public float capacity;
    [Range(0, 500f)]
    public float maxEnergy;
    [Range(0,10f)]
    public float energyRegen;
    [Range(0, 10f)]
    public float drainRate;
    public float energy;
    public float nectar;
    [Range(0, 10f)]
    public float waitRadius;
    [Range(0, 10f)]
    public float searchRadius;
    
    public bool hasSearched;
    public bool atTarget;
    public Slider energyBar;
    public Slider nectarBar;
    public GameObject infoPanel;
    public BeeState state;
    public Transform home;
    public List<Transform> targets;
    
    
    // private/hidden variables
    private SpriteRenderer render;
    
    private float rotationAngle;
    //[HideInInspector]
    public Vector3 target;
    [HideInInspector]
    public string id;
    private List<Collider2D> array;
    public float waitCount;
    private List<Flower.Species> harvestedFrom;
    
    // enumerated values
    public enum BeeState
    {
        Wait,
        Harvest,
        Full,
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
        infoPanel.SetActive(false);
        rotationAngle = 0;
        targets = new List<Transform>();
        hasSearched = true;
        waitCount = 0;
        harvestedFrom = new List<Flower.Species>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case BeeState.Wait:
                fly();
                waitOrSearch();
                break;
            case BeeState.Harvest:
                fly();
                searchArea();
                harvestNectar();
                break;
            case BeeState.Full:
                fly();
                returnNectar();
                break;
        }

        energyBar.maxValue = maxEnergy;
        energyBar.value = energy;
        nectarBar.maxValue = capacity;
        nectarBar.value = nectar;

        if(nectar >= capacity)
        {
            target = home.GetComponent<Hive>().dropoff.position;
            state = BeeState.Full;
        }
    }

    public void fly() // fly to the target and circle it once there
    {
        if (Vector3.Distance(target, transform.position) > waitRadius) // if target is farther away than .1 units, move to it
        {
            flyToTarget();
            atTarget = false;
        }
        else
        {
            flyAroundTarget(); // otherwise fly around it
            atTarget = true;
        }
    }

    public void flyToTarget()
    {
        if(energy >= 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, flySpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, target, flySpeed * .5f * Time.deltaTime);
        }

        float drain = drainRate * Time.deltaTime;
        if(energy - drain > 0)
        {
            energy -= drain;
        }
        else
        {
            energy = 0;
        }
        
    }

    public void flyAroundTarget()
    {
        Vector3 offset = new Vector3(Mathf.Sin(rotationAngle) * waitRadius, Mathf.Cos(rotationAngle) * waitRadius, 0);
        if(energy >= 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, target + offset, Time.deltaTime * .5f * waitSpeed);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, target + offset, Time.deltaTime * .5f * waitSpeed);
        }
        rotationAngle += Time.deltaTime * waitSpeed;

        float drain = drainRate * .75f * Time.deltaTime;
        if(energy - drain > 0)
        {
            energy -= drain;
        }
        else
        {
            energy = 0;
        }
        
    }

    public void waitOrSearch()
    {
        waitCount += waitTime * Time.deltaTime;
        if (!hasSearched || waitCount >= waitTime) // search the area every 120 frames ~= 2 seconds
        {
            hasSearched = false;
            searchArea();
            waitCount = 0;
        }
    }

    public void returnNectar() // if at the hive, drop off nectar and return to the last known target location
    {
        if(atTarget)
        {
            if (energy < energyBar.maxValue)
            {
                energy += energyRegen * 2 * Time.deltaTime;
            }
            float a = drinkSpeed * Time.deltaTime;
            bool canDrop = home.GetComponent<Hive>().addHoney(a, harvestedFrom);
            if (canDrop)
            {
                nectar -= a;
                if (nectar <= 0)
                {
                    nectar = 0;
                    if(targets.Count > 0 && targets[0].gameObject.activeSelf)
                    {
                        harvestedFrom.Clear();
                        target = targets[0].position;
                        state = BeeState.Harvest;
                    }
                    else
                    {
                        harvestedFrom.Clear();
                        nextTarget();
                    }
                }
            }
        }
    }

    public void searchArea()
    {

        if (atTarget && !hasSearched) // if at the target, find nearby flowers
        {
            hasSearched = true;
            Collider2D[] array = Physics2D.OverlapCircleAll(transform.position, searchRadius);
            targets.Clear();
            foreach(Collider2D c in array)
            {
                if(c.gameObject.tag == "flower") // if the collider is on a flower game object
                {
                    //Debug.Log(c);
                    targets.Add(c.gameObject.transform);
                }
                if(c.gameObject.tag == "hive") // if a hive is found, clear the list and only add the hive as a target
                {
                    targets.Clear();
                    targets.Add(c.gameObject.transform);
                    break;
                }
            }
            if(targets.Count == 1 && targets[0].gameObject.tag == "hive")
            {
                state = BeeState.Wait;
                target = targets[0].GetComponent<Hive>().wait.transform.position;
            }
            else if (targets.Count > 0)
            {
                shuffle(targets);
                target = targets[0].position;
                state = BeeState.Harvest;
            }
            //Debug.Log($"{id} has found {targets.Count} targets");
        }
    }

    public void harvestNectar()
    {
        if(atTarget)
        {
            if(targets.Count > 0 && targets[0].gameObject.activeSelf) // if the current target is still active in game
            {
                Flower f = targets[0].GetComponent<Flower>(); // get flower comp
                float draw = drinkSpeed * Time.deltaTime; // drink a random amount
                if(f.drinkNectar(draw)) // if able to drink nectar
                {
                    nectar += draw; // add that amount
                    if (energy < energyBar.maxValue)
                    {
                        energy += energyRegen * Time.deltaTime;
                    }
                    if(!harvestedFrom.Contains(f.species))
                    {
                        harvestedFrom.Add(f.species);
                    }
                }
                else
                {
                    nextTarget(); // otherwise select next target
                }
            }
            else
            {
                nextTarget(); // otherwise select next target
            }
        }
        
    }

    private void nextTarget()
    {
        targets.RemoveAt(0); // remove the target from the list
        if(targets.Count > 0) // if there are still targets left
        {
            target = targets[0].position; // move selecte the next one
        }
        else
        { 
            hasSearched = false; // otherwise shift into waiting state and immediatly search for new targets
            state = BeeState.Wait;
        }
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

    // Adpated from https://stackoverflow.com/questions/273313/randomize-a-listt
    private static void shuffle(List<Transform> list)  
    {  
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = (int) Math.Floor((double) UnityEngine.Random.Range(0, n + 1));  
            Transform value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }  
    }
}
