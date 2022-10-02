using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hive : MonoBehaviour
{
    public int maxHoney;
    public Slider honeyBar;
    public Button spawnButton;
    public GameObject p_bee;

    // Start is called before the first frame update
    void Start()
    {
        honeyBar.maxValue = maxHoney;
        honeyBar.value = 0;
        spawnButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(honeyBar.value >= honeyBar.maxValue)
        {
            spawnButton.gameObject.SetActive(true);
        }
        else
        {
            spawnButton.gameObject.SetActive(false);
        }
    }

    public void SpawnButton()
    {
        honeyBar.value -= honeyBar.value; // zero out the bar value
        honeyBar.maxValue += 2000;
        StartCoroutine(SpawnBees()); // spawn some bees
    }

    public IEnumerator SpawnBees()
    {
        for(int i = 0; i < 5; i++)
        {
            GameObject temp = Instantiate(p_bee, transform.position + new Vector3(2, 0, 0), Quaternion.identity);
            Bee b = temp.GetComponent<Bee>();
            b.target = transform.position;
            b.home = transform;
            b.id = $"Bee{i}";
            yield return new WaitForSeconds(.75f);
        }
    }
}
