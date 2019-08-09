using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cloudSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject clonedGameObj;
    private float backgroundWidth;
    RectTransform backgroundTs;
    public float scroll_speed; 
    
    void Start()
    {
        clonedGameObj = Instantiate(GameObject.Find("cloud"));
        transform.position = new Vector3(0, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (clonedGameObj.transform.position.x == backgroundWidth)
        {
            Destroy(clonedGameObj);
        }
    }
}
