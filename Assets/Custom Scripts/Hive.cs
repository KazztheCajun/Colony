using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hive : MonoBehaviour
{
    [Range(0,5000f)]
    public float maxHoney;
    [Range(0,5000f)]
    public float maxWater;
    [Range(0,10f)]
    public float expScale;
    [Range(0,200f)]
    public float expStart;
    [Range(0,25)]
    public int flySpeedPerLevel;
    [Range(0,25)]
    public int waitSpeedPerLevel;
    [Range(0,50f)]
    public float capacityPerLevel;
    [Range(0,50f)]
    public float energyPerLevel;
    [Range(0,50f)]
    public int drinkPerLevel;
    [Range(0,5f)]
    public float regenPerLevel;
    [Range(0,5f)]
    public float drainPerLevel;
    public Slider honeyBar;
    public Slider waterBar;
    public Slider speedExp;
    public Slider capacityExp;
    public Slider energyExp;
    public Slider drinkExp;
    public Slider regenExp;
    public Slider drainExp;
    public Button speedExpButton;
    public Button capacityExpButton;
    public Button energyExpButton;
    public Button drinkExpButton;
    public Button regenExpButton;
    public Button drainExpButton;
    public Button spawnButton;
    public GameObject p_bee;
    public Transform dropoff;
    public Transform spawn;
    public Transform wait;

    // private variables
    private List<Bee> spawnedBees;
    

    // Start is called before the first frame update
    void Start()
    {
        honeyBar.maxValue = maxHoney;
        honeyBar.value = 0;
        waterBar.maxValue = maxWater;
        waterBar.value = maxWater;
        spawnButton.gameObject.SetActive(false);
        speedExpButton.gameObject.SetActive(false);
        capacityExpButton.gameObject.SetActive(false);
        energyExpButton.gameObject.SetActive(false);
        drinkExpButton.gameObject.SetActive(false);
        regenExpButton.gameObject.SetActive(false);
        drainExpButton.gameObject.SetActive(false);
        spawnedBees = new List<Bee>();
        speedExp.maxValue = expStart;
        speedExp.value = 0;
        capacityExp.maxValue = expStart;
        capacityExp.value = 0;
        energyExp.maxValue = expStart;
        energyExp.value = 0;
        drinkExp.maxValue = expStart;
        drinkExp.value = 0;
        regenExp.maxValue = expStart;
        regenExp.value = 0;
        drainExp.maxValue = expStart;
        drainExp.value = 0;
        StartCoroutine(SpawnBees(spawnedBees));
    }

    // Update is called once per frame
    void Update()
    {
        if(speedExp.value >= speedExp.maxValue)
        {
            speedExpButton.gameObject.SetActive(true);
        }
        else
        {
            speedExpButton.gameObject.SetActive(false);
        }
        if(capacityExp.value >= capacityExp.maxValue)
        {
            capacityExpButton.gameObject.SetActive(true);
        }
        else
        {
            capacityExpButton.gameObject.SetActive(false);
        }
        if(energyExp.value >= energyExp.maxValue)
        {
            energyExpButton.gameObject.SetActive(true);
        }
        else
        {
            energyExpButton.gameObject.SetActive(false);
        }
        if(drinkExp.value >= drinkExp.maxValue)
        {
            drinkExpButton.gameObject.SetActive(true);
        }
        else
        {
            drinkExpButton.gameObject.SetActive(false);
        }
        if(regenExp.value >= regenExp.maxValue)
        {
            regenExpButton.gameObject.SetActive(true);
        }
        else
        {
            regenExpButton.gameObject.SetActive(false);
        }
        if(drainExp.value >= drainExp.maxValue)
        {
            drainExpButton.gameObject.SetActive(true);
        }
        else
        {
            drainExpButton.gameObject.SetActive(false);
        }

        if(honeyBar.value >= honeyBar.maxValue)
        {
            spawnButton.gameObject.SetActive(true);
        }
        else
        {
            spawnButton.gameObject.SetActive(false);
        }
    }

    public bool addHoney(float amount, List<Flower.Species> f)
    {
        if(f.Contains(Flower.Species.PEA))
        {
            speedExp.value += Time.deltaTime * expScale;
        }
        if(f.Contains(Flower.Species.LILAC))
        {
            capacityExp.value += Time.deltaTime * expScale;
        }
        if(f.Contains(Flower.Species.TULIP))
        {
            energyExp.value += Time.deltaTime * expScale;
        }
        if(f.Contains(Flower.Species.MARIGOLD))
        {
            drinkExp.value += Time.deltaTime * expScale;
        }
        if(f.Contains(Flower.Species.MORNING))
        {
            regenExp.value += Time.deltaTime * expScale;
        }
        if(f.Contains(Flower.Species.SNAP))
        {
            drainExp.value += Time.deltaTime * expScale;
        }

        if(honeyBar.value + amount <= honeyBar.maxValue)
        {
            honeyBar.value += amount;
            return true;
        }
        else
        {
            honeyBar.value = honeyBar.maxValue;
            return false;
        }
            
    }

    public void SpawnButton()
    {
        honeyBar.value -= honeyBar.value; // zero out the bar value
        honeyBar.maxValue += 2000;
        StartCoroutine(SpawnBees(spawnedBees)); // spawn some bees
    }

    public void UpgradeBees(string attribute)
    {
        foreach(Bee b in spawnedBees)
        {
            switch (attribute)
            {
                case "speed":
                    b.flySpeed += flySpeedPerLevel;
                    b.waitSpeed += waitSpeedPerLevel;
                    speedExp.value = speedExp.value - speedExp.maxValue;
                    speedExp.maxValue += 50;
                    break;
                case "capacity":
                    b.nectarBar.maxValue += capacityPerLevel;
                    capacityExp.value = capacityExp.value - capacityExp.maxValue;
                    capacityExp.maxValue += 50;
                    break;
                case "energy":
                    b.energyBar.maxValue += energyPerLevel;
                    energyExp.value = energyExp.value - energyExp.maxValue;
                    energyExp.maxValue += 50;
                    break;
                case "drink":
                    b.drinkSpeed += drinkPerLevel;
                    drinkExp.value = drinkExp.value - drinkExp.maxValue;
                    drinkExp.maxValue += 50;
                    break;
                case "regen":
                    b.energyRegen += regenPerLevel;
                    regenExp.value = regenExp.value - regenExp.maxValue;
                    regenExp.maxValue += 50;
                    break;
                case "drain":
                    b.drainRate -= drainPerLevel;
                    drainExp.value = drainExp.value - drainExp.maxValue;
                    drainExp.maxValue += 50;
                    break;
            }
        }
    }

    public IEnumerator SpawnBees(List<Bee> l)
    {
        // generate a list of points in a circular area
        List<Vector3> temp = GameObject.Find("SelectionController").GetComponent<SelectController>().GetPositionsAround(new Vector3(wait.position.x, wait.position.y, 1), new float[] {1f, 2f, 3f}, new int[] {5, 10, 20});
        int index = 0;
        for(int i = 0; i < 5; i++)
        {
            GameObject t = Instantiate(p_bee, spawn.position, Quaternion.identity);
            Bee b = t.GetComponent<Bee>();
            l.Add(b);
            b.target = temp[index];
            index = (index + 1) % temp.Count;
            b.home = transform;
            b.id = $"Bee{spawnedBees.Count}";
            yield return new WaitForSeconds(.75f);
        }
    }
}
