using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobNPCBehaviour : MonoBehaviour
{
    public string jobName;
    public string jobDisplayedName;
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

    public void GainJob()
    {
        GameManager.instance.GainProductionJob(jobName, jobDisplayedName, true);
    }
}
