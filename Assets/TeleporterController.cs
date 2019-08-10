using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterController : MonoBehaviour
{
    Animator _animator = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Teleport()
    {
        if (!_animator)
        {
            _animator = gameObject.GetComponent<Animator>();
        }
        _animator.SetTrigger("teleport");
    }
}
