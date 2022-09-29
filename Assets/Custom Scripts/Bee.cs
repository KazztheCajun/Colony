using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bee : MonoBehaviour
{
    // public variables
    public int flySpeed;
    public int waitSpeed;
    public int drinkSpeed;
    public int capacity;
    public int maxEnergy;
    public float waitRadius;
    public float searchRadius;
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
    //[HideInInspector]
    public int nectar;
    private bool atTarget;
    [HideInInspector]
    public bool hasSearched;
    private List<Transform> targets;
    [HideInInspector]
    public Transform home;
    private List<Collider2D> array;
    
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
        target = transform.position;
        infoPanel.SetActive(false);
        rotationAngle = 0;
        targets = new List<Transform>();
        hasSearched = true;
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
                searchArea();
                harvestNectar();
                break;
            case BeeState.Full:
                fly();
                returnNectar();
                break;
        }

        

        energyBar.value = energy;
        nectarBar.value = nectar;

        if(nectar >= capacity)
        {
            target = home.position;
            state = BeeState.Full;
        }
    }

    public void fly()
    {
        if (Vector3.Distance(target, transform.position) > waitRadius) // if target is farther away than .1 units, move to it
        {
            flyToTarget();
            atTarget = false;
        }
        else
        {
            flyAroundTarget();
            atTarget = true;
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

    public void returnNectar()
    {
        if(Vector3.Distance(transform.position, home.position) - waitRadius <= .1f)
        {
            home.GetComponent<Hive>().honeyBar.value += nectar;
            nectar = 0;
            if(targets.Count >= 1)
            {
                target = targets[0].position;
                state = BeeState.Harvest;
            }
            else
            {
                state = BeeState.Wait;
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
            }
            if (targets.Count > 0)
            {
                target = targets[0].position;
            }
            Debug.Log($"{id} has found {targets.Count} targets");
        }
    }

    public void harvestNectar()
    {
        if(atTarget && Time.frameCount % 120 == 0 && targets.Count > 0)
        {
            if(targets[0].gameObject.tag == "flower")
            {
                Flower f = targets[0].GetComponent<Flower>(); // get flower comp
                int draw = Random.Range(drinkSpeed/3, drinkSpeed); // drink a random amount
                if(f.drinkNectar(draw))
                {
                    nectar += draw;
                }
                else
                {
                    targets.RemoveAt(0);
                    if(targets.Count > 0)
                    {
                        target = targets[0].position;
                    }
                    else
                    {
                        hasSearched = false;
                    }
                    
                }
            }
            
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
}
