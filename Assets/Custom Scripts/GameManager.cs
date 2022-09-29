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
    public bool play;
    public bool mousePan;
    public int buffer;
    public List<Collider2D> spawnAreas;

    // In Game references

    private GameObject hive;
    private List<GameObject> bees;
    private List<GameObject> flowers;

    // Start is called before the first frame update
    void Start()
    {
        if(play)
        {
            bees = new List<GameObject>();
            flowers = new List<GameObject>();
            hive = Instantiate(p_hive);
            StartCoroutine(SpawnBees());
            spawnGrass();
        }
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void spawnGrass()
    {
        int count = 0;
        foreach(Collider2D c in spawnAreas)
        {
            bool isValid = false;
            Vector3 point = Vector3.zero;
            while (!isValid)
            {
                point = new Vector3(Random.Range(c.bounds.min.x + buffer, c.bounds.max.x - buffer), Random.Range(c.bounds.min.y + buffer, c.bounds.max.y - buffer), 1);
                isValid = c.OverlapPoint(point);
            }

            for (int i = 0; i < (buffer * 10); i++)
            {
                isValid = true;
                Vector3 loc = point + ((Vector3) Random.insideUnitCircle * buffer);
                Collider2D[] t = Physics2D.OverlapCircleAll(loc, 2f);
                foreach(Collider2D temp in t)
                {
                    if (temp.gameObject.tag == "grass")
                    {
                        isValid = false;
                        //Debug.Log($"{temp} unable to spawn.");
                    }
                }
                if (isValid)
                {
                    GameObject temp = Instantiate(p_grass, point + ((Vector3) Random.insideUnitCircle * buffer), Quaternion.identity);
                    if(Random.value >= .4)
                    {
                        Instantiate(p_flowers[count], temp.transform.position + ((Vector3) Random.insideUnitCircle), Quaternion.identity);
                    }
                }
            }
            count++;
        }
    }

    IEnumerator SpawnBees()
    {
        for(int i = 0; i < 5; i++)
        {
            GameObject temp = Instantiate(p_bee, hive.transform.position + new Vector3(2, 0, 0), Quaternion.identity);
            Bee b = temp.GetComponent<Bee>();
            b.target = hive.transform.position;
            b.home = hive.transform;
            b.id = $"Bee #{i}";
            bees.Add(temp);
            yield return new WaitForSeconds(.75f);
        }
    }
}
