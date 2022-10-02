using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flower : MonoBehaviour
{
    public double nectar;
    public double maxNectar;
    public int lifeTime;
    public GameObject seed;
    public Color norm;
    public Color selected;
    public GameObject infoPanel;
    public Slider nectarBar;
    private SpriteRenderer render;
    private bool pollinated;
    private int spawnTime;

    // Start is called before the first frame update
    void Start()
    {
        nectar = maxNectar;
        render = GetComponent<SpriteRenderer>();
        nectarBar.maxValue = (float) maxNectar;
        nectarBar.value = (float) maxNectar;
        infoPanel.SetActive(false);
        spawnTime = Time.frameCount;
        lifeTime = Random.Range(1800, 2600);
    }

    // Update is called once per frame
    void Update()
    {
        if ((Time.frameCount % 60 == 0) && (nectar < maxNectar))
        {
            double t = nectar;
            nectar += maxNectar * .001;
            //Debug.Log($"{this} regains {nectar - t} nectar.");
        }
        nectarBar.value = (float) nectar;
        if(Time.frameCount - spawnTime >= lifeTime)
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

    public bool drinkNectar(int amount)
    {
        if (nectar - amount > 20f)
        {
            nectar -= amount;
            return true;
        }
        else
        {
            pollinated = true;
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
}
