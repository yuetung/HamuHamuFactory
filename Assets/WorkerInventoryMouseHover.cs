using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WorkerInventoryMouseHover : MonoBehaviour, IPointerClickHandler
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
        transform.parent.gameObject.GetComponent<WorkerInventoryItemController>().Click();
    }

    public void OnDrag(PointerEventData eventData)
    {
        //print("I'm being dragged!");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.parent.gameObject.GetComponent<WorkerInventoryItemController>().PointerEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.parent.gameObject.GetComponent<WorkerInventoryItemController>().PointerExit();
    }
}
