using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cloudWave : MonoBehaviour
{
    // Start is called before the first frame update
    public float range;
    public float speed;
    //private float period;
    void Start()
    {
        //period =  Random.Range(0,2*Mathf.PI);
    }

    // Update is called once per frame
    void Update()
    {

        //calculate what the new Y position will be
        float newY = Mathf.Sin(Time.time * speed)*range;
        //set the object's Y to the new calculated Y
        transform.Translate(0,newY,0);
    }
}
