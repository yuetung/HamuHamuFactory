using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// for animation events: lights, sound, etc
/// </summary>
public class AnimSynchro : MonoBehaviour
{
    [SerializeField]
    private List<Animator> childAnimators;

    public bool isBlinking = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBoolAnimEvent(bool x)
    {
        foreach (Animator anim in childAnimators)
        {
            //set this to a generalised parameter
            anim.SetBool("isBlinking", x);
        }
    }
}
