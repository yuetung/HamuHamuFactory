using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CloudScroll : MonoBehaviour
{
   
    public float scroll_speed; 
    RectTransform backgroundTs;
    private GameObject clonedGameObj;
    
    private void Start() {
        transform.position = new Vector3(0, transform.position.y);
        clonedGameObj = Instantiate(GameObject.Find("cloud"));
    }

    void Update()
    {
        //clonedGameObj.transform.position = new Vector3(clonedGameObj.transform.position.x + scroll_speed, clonedGameObj.transform.position.y);
        //transform.Translate(scroll_speed,0,0);
    }
}
