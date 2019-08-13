using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAndPause : MonoBehaviour
{
    public float pauseTime = 2.0f;
    public float transitionTime = 0.5f;
    public float rotationDegrees = 90.0f;
    private RectTransform pivotRect;
    private float dR;

    // Start is called before the first frame update
    void Start()
    {
        dR = rotationDegrees / transitionTime;
        pivotRect = this.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        //pivotRect.RotateAroundLocal(new Vector3(0, 0, 1), dR);
    }
    
}
