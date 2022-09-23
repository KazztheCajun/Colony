using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // prefab references
    public GameObject p_hive;
    public GameObject p_bee;
    public GameObject p_grass;
    public List<GameObject> p_flowers;

    // In Game references

    private GameObject hive;
    private List<GameObject> bees;
    private List<GameObject> flowers;

    // Start is called before the first frame update
    void Start()
    {
        bees = new List<GameObject>();
        flowers = new List<GameObject>();
        hive = Instantiate(p_hive);

        StartCoroutine(SpawnBees());

        spawnGrass(new Vector3(20, 20, 1), 5);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void spawnGrass(Vector3 center, int radius)
    {
        //int num = Random.Range(30, 50);
        for (int i = 0; i < (radius * 25); i++)
        {
            GameObject temp = Instantiate(p_grass, center + ((Vector3) Random.insideUnitCircle * radius), Quaternion.identity);
            if(Random.value >= .5)
            {
                Instantiate(p_flowers[0], temp.transform.position + ((Vector3) Random.insideUnitCircle * temp.transform.localScale.x), Quaternion.identity);
            }
        }
    }

    IEnumerator SpawnBees()
    {
        for(int i = 0; i < 5; i++)
        {
            GameObject temp = Instantiate(p_bee, hive.transform.position + new Vector3(2, 0, 0), Quaternion.identity);
            Bee b = temp.GetComponent<Bee>();
            b.target = hive.transform;
            b.id = $"Bee #{i}";
            bees.Add(temp);
            yield return new WaitForSeconds(.75f);
        }
    }
}
