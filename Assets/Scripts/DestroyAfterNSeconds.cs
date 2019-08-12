﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterNSeconds : MonoBehaviour
{
    public float time = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, time);   
    }
}
