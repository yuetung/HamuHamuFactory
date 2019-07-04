using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryMouseHover : MonoBehaviour, IPointerClickHandler 
     , IDragHandler
     , IPointerEnterHandler
     , IPointerExitHandler
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData) // 3
    {
        //print("I was clicked");
    }

    public void OnDrag(PointerEventData eventData)
    {
        //print("I'm being dragged!");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.parent.gameObject.GetComponent<InventoryItemController>().ShowName();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.parent.gameObject.GetComponent<InventoryItemController>().DisableName();
    }
}
