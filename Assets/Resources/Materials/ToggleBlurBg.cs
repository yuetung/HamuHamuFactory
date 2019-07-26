using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleBlurBg : MonoBehaviour
{
    void OnDisable()
    {
        Debug.Log("PrintOnDisable: script was disabled");
        this.gameObject.transform.parent.GetComponent<RawImage>().enabled = false;
    }

    void OnEnable()
    {
        Debug.Log("PrintOnEnable: script was enabled");
        this.gameObject.transform.parent.GetComponent<RawImage>().enabled = true;
    }
}
