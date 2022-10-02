using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{
    public GameObject p_grass;
    public Collider2D area;
    public Collider2D playArea;
    public bool canGrow;
    public float growRate;
    public float growRadius;
    public int lastGrow;


    // Start is called before the first frame update
    void Start()
    {
        lastGrow = Time.frameCount;
        growRate = Random.Range(1500, 2500);
        Color c = Random.value <= .5 ? new Color(Random.Range(0.0f, 0.5f), 0.6320754f, Random.Range(0.0f, 0.2f)) : new Color(Random.Range(0.0f, 0.2f), 0.6320754f, Random.Range(0.0f, 0.5f)); 
        GetComponent<SpriteRenderer>().color =c;
        playArea = GameObject.Find("Background").GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(canGrow && Time.frameCount - lastGrow >= growRate && Random.value >= .99)
        {
            bool isValid = true;
            Vector3 point = Vector3.zero;
            // draw a circle for the spawn area and find a valid starting point
            point = this.transform.position +  ((Vector3) Random.insideUnitCircle * growRadius);
            Collider2D[] t = Physics2D.OverlapCircleAll(point, 1f); 
            foreach(Collider2D temp in t)
            {
                if (temp.gameObject.tag == "grass") // check if grass already exists in this area
                {
                    isValid = false;
                    //Debug.Log($"{temp} unable to spawn.");
                }
            }
            if (isValid && playArea.OverlapPoint(point)) // spawn grass
            {
                GameObject g = Instantiate(p_grass, point, Quaternion.identity);
                g.GetComponent<Grass>().area = this.area;
                lastGrow = Time.frameCount;
            }
            else if (t.Length > 20)
            {
                canGrow = false;
            }
            
        }
    }
}
