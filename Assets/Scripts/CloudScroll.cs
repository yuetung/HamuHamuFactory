using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CloudScroll : MonoBehaviour
{
   
    public float scroll_speed; 
    public float background_width;
    
    private void Start() {
    }

    void Update()
    {
        if(transform.position.x >= background_width){
            Destroy(this.gameObject);
            Debug.Log("cloud has been destroyed");
        }
        transform.Translate(scroll_speed,0,0);
    }
}
