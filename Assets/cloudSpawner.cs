using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cloudSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject gameObject1ToMove;
    public GameObject gameObject2ToMove;
    public float waitTimeFrom;
    public float waitTimeTo;
    public float scroll_speed_min; 

    public float scroll_speed_max;
    
    public float screen_cover_ratio;
    
    private float backgroundWidth;
    private float backgroundPosStarting;

    private float backgroundCelling;
    private float backgroundFloor;
    RectTransform backgroundTs;

    Vector3 starting_position;

    GameObject[] randomClouds = new GameObject[2];
    
    void Start()
    {
        // Get background dimentional params
        backgroundTs= (RectTransform)GameObject.Find("background").transform;
        backgroundWidth = backgroundTs.rect.width;
        backgroundPosStarting = backgroundTs.rect.position.x;
        backgroundCelling = backgroundTs.rect.yMax;
        backgroundFloor = backgroundTs.rect.yMin;
        randomClouds[0] = gameObject1ToMove;
        randomClouds[1] = gameObject2ToMove;
        // Start generating with certain interval
        StartCoroutine(SpawnForever(Random.Range(waitTimeFrom, waitTimeTo)));
    }

    // Update is called once per frame
    private IEnumerator SpawnForever(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            // Starting at the beginning of the background image, with could covering certain % from the top
            starting_position = new Vector3(transform.position.x, Random.Range((backgroundCelling-(backgroundCelling-backgroundFloor)*screen_cover_ratio),backgroundCelling));
            GameObject tempClone = Instantiate(randomClouds[Random.Range(0,2)],starting_position,this.transform.rotation,this.gameObject.transform);
            tempClone.GetComponent<CloudScroll>().scroll_speed = Random.Range(scroll_speed_min, scroll_speed_max);
            tempClone.GetComponent<CloudScroll>().background_width = backgroundWidth;
        }
    }
}
