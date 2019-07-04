using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform playerPos;
    public MainCharacterController mainCharacterController;
    private float camHalfWidth = 0;
    [Tooltip("set it to slightly higher than player body width")]
    public float delta = 500f;

    // Start is called before the first frame update
    void Start()
    {
        camHalfWidth = GetComponent<Camera>().orthographicSize * ((float)Screen.width / Screen.height) - delta;
        //Debug.Log(camHalfWidth);
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 v = transform.position;
        if (playerPos.position.x > mainCharacterController.maxPos - camHalfWidth)
        {
            v.x = mainCharacterController.maxPos - camHalfWidth;
            
        }
        else if (playerPos.position.x < mainCharacterController.minPos + camHalfWidth)
        {
            v.x = mainCharacterController.minPos + camHalfWidth;
        }
        else
        {
            v.x = playerPos.transform.position.x;
        }
        gameObject.transform.position = v;
    }
}
