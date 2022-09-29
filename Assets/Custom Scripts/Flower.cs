using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flower : MonoBehaviour
{
    public double nectar;
    public double maxNectar;
    public Transform seed;
    public Color norm;
    public Color selected;
    public GameObject infoPanel;
    public Slider nectarBar;
    private SpriteRenderer render;

    // Start is called before the first frame update
    void Start()
    {
        nectar = maxNectar;
        render = GetComponent<SpriteRenderer>();
        nectarBar.maxValue = (float) maxNectar;
        nectarBar.value = (float) maxNectar;
        infoPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if ((Time.frameCount % 180 == 0) && (nectar < maxNectar))
        {
            double t = nectar;
            nectar += maxNectar * .001;
            Debug.Log($"{this} regains {nectar - t} nectar.");
        }
        nectarBar.value = (float) nectar;
    }

    public bool drinkNectar(int amount)
    {
        if (nectar - amount > 10f)
        {
            nectar -= amount;
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
}
