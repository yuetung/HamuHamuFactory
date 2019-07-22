using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialPickupBehaviour : MonoBehaviour
{
    public string materialName;
    public string displayedName;
    public int amount;
    public DayOfWeek[] appearingDays;

    // Start is called before the first frame update
    void Start()
    {
        bool appear = false;
        foreach (DayOfWeek day in appearingDays)
        {
            if (DateTime.Now.DayOfWeek.Equals(day))
            {
                appear = true;
            }
        }
        gameObject.SetActive(appear);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PickUp()
    {
        GameManager.instance.GainMaterial(materialName, amount, displayedName, true);
    }
}
