using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public Collider2D hiveSpawn;
    public List<Collider2D> spawnAreas;
    //public TextMeshProUGUI UIText;
    

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
            spawnHive();
            //StartCoroutine(hive.GetComponent<Hive>().SpawnBees());
            spawnGrass();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        //UIText.text = $"{Time.frameCount}";
    }

    private void spawnHive()
    {
        // find a point in the spawn area
        Vector3 point = new Vector3(Random.Range(hiveSpawn.bounds.min.x, hiveSpawn.bounds.max.x), Random.Range(hiveSpawn.bounds.min.y, hiveSpawn.bounds.max.y), 1);
        // spawn hive there
        hive = Instantiate(p_hive, point, Quaternion.identity);
        Camera.main.transform.position = hive.transform.position - new Vector3(0, 0, 1);
    }

    private void spawnGrass()
    {
        int count = 0;
        foreach(Collider2D c in spawnAreas) // loop through each spawn area and spawn a patch of grass
        {
            bool isValid = false;
            Vector3 point = Vector3.zero;
            while (!isValid) // find a valid point inside the collider
            {
                point = new Vector3(Random.Range(c.bounds.min.x + buffer, c.bounds.max.x - buffer), Random.Range(c.bounds.min.y + buffer, c.bounds.max.y - buffer), 1);
                isValid = c.OverlapPoint(point);
            }

            for (int i = 0; i < (buffer * 10); i++) // depending on the radius of the spawn zone, spawn a buch of grass
            {
                isValid = true;
                Vector3 loc = point + ((Vector3) Random.insideUnitCircle * buffer);
                Collider2D[] t = Physics2D.OverlapCircleAll(loc, 2f); 
                foreach(Collider2D temp in t)
                {
                    if (temp.gameObject.tag == "grass") // check if grass already exists in this area
                    {
                        isValid = false;
                        //Debug.Log($"{temp} unable to spawn.");
                    }
                }
                if (isValid) // if not, spawn the grass
                {
                    GameObject temp = Instantiate(p_grass, point + ((Vector3) Random.insideUnitCircle * buffer), Quaternion.identity);
                    temp.GetComponent<Grass>().area = c;
                    if(Random.value >= .4) // and 60% of the time spawn a flower
                    {
                        GameObject g = Instantiate(p_flowers[count], temp.transform.position + ((Vector3) Random.insideUnitCircle), Quaternion.identity);
                        g.transform.GetChild(0).transform.Rotate(0,0,Random.Range(0, 360));
                    }
                }
            }
            count++;
        }
    }

    public void ExitGame()
    {
        SceneManager.LoadScene("MenuScreen");
    }
}
