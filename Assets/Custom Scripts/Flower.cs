using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flower : MonoBehaviour
{    

    // Enumerated Values
    public enum Species {MORNING, LILAC, SNAP, MARIGOLD, PEA, TULIP}

    // public variables
    public Species species;
    [Range(0, 5000f)]
    public float maxNectar;
    [Range(0, 500f)]
    public float lifeTime;
    public GameObject seed;
    public Color norm;
    public Color selected;
    public GameObject infoPanel;
    public Slider nectarBar;

    // private variables
    private SpriteRenderer render;
    private bool pollinated;
    private float spawnTime;

    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<SpriteRenderer>();
        nectarBar.maxValue = (float) maxNectar;
        nectarBar.value = (float) maxNectar;
        infoPanel.SetActive(false);
        spawnTime = 0;
        lifeTime = Random.Range(30, 40);
    }

    // Update is called once per frame
    void Update()
    {
        spawnTime += Time.deltaTime;
        if(spawnTime >= lifeTime)
        {
            this.gameObject.SetActive(false);

            if(pollinated)
            {
                int num = Random.Range(2, 4);
                for(int i = 0; i < num; i++)
                {
                    spawnSeed();
                }
            }
            else
            {
                spawnSeed();
            }
            decompose();
        }

        if(nectarBar.value < 50f && !pollinated)
        {
            pollinated = true;
        }
    }

    private void spawnSeed()
    {
        bool isValid = false;
        int count = 0;
        int loop = 0;
        Vector3 point = Vector3.zero;
        while (!isValid && loop < 20)
        {
            loop++;
            point = this.transform.position + ((Vector3) Random.insideUnitCircle * 2f);
            Collider2D[] t = Physics2D.OverlapCircleAll(point, 1f);
            count = 0;
            foreach(Collider2D c in t)
            {
                if(c.gameObject.tag == "seed" || c.gameObject.tag == "flower")
                {
                    count++;
                }
            }
            if(count == 0)
            {
                isValid = true;
            }
        }
        if (isValid)
        {
            GameObject t = Instantiate(seed, point, Quaternion.identity);
            t.transform.Rotate(0,0,Random.Range(0, 360));
        }
    }

    public bool drinkNectar(float amount)
    {
        if (nectarBar.value - amount >= 0)
        {
            nectarBar.value -= amount;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void selectFlower()
    {
        //Debug.Log($"{id} has been selected.");
        infoPanel.SetActive(true);
        //render.color = selected;
    }

    public void deselectFlower()
    {
        //Debug.Log($"{id} has been deselected.");
        infoPanel.SetActive(false);
        //render.color = norm;
    }

    public IEnumerator decompose()
    {
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
    }
}
