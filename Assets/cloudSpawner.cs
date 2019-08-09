using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cloudSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject gameObjectToMove;
    public float waitTime;
    public float scroll_speed; 
    
    private float backgroundWidth;
    RectTransform backgroundTs;

    Vector3 starting_position;
    
    void Start()
    {
        backgroundTs= (RectTransform)GameObject.Find("background").transform;
        backgroundWidth =  backgroundTs.rect.width;
        Debug.Log("new cloud created");
        StartCoroutine(SpawnForever(waitTime));
    }

    // Update is called once per frame
    private IEnumerator SpawnForever(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            starting_position = new Vector3(0, transform.position.y);
            GameObject tempClone = Instantiate(gameObjectToMove,starting_position,this.transform.rotation,this.gameObject.transform);
            tempClone.GetComponent<CloudScroll>().scroll_speed = scroll_speed;
            tempClone.GetComponent<CloudScroll>().background_width = backgroundWidth;
        }
    }
}
