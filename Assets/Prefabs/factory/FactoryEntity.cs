using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryEntity : MonoBehaviour
{
    [SerializeField]
    private List<Animator> childAnimators;

    //test variables: please delete and use function in future
    public bool isStarted=false;
    public bool isOperating = false;
    public bool triggerAction = false;
    bool prev = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        IsStarted(isStarted);
        IsOperating(isOperating);
        if (triggerAction != prev)
        {
            TriggerAction();
            prev = triggerAction;
        }
    }
    
    public void IsStarted(bool x)
    {
        if (childAnimators != null)
            foreach (Animator anim in childAnimators)
            {
                anim.SetBool("isStarted", x);
            }
    }

    public void IsOperating(bool x)
    {
        if (childAnimators != null)
            foreach (Animator anim in childAnimators)
            {
                anim.SetBool("isOperating", x);
            }
    }

    public void TriggerAction()
    {
        if (childAnimators != null)
            foreach (Animator anim in childAnimators)
            {
                anim.SetTrigger("triggerAction");
            }
    }
}
