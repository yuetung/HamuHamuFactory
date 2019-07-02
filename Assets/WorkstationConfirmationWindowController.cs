using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkstationConfirmationWindowController : MonoBehaviour
{
    private WorkstationShopItemController currentShopItemController;
    public Text _confirmationText;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowConfirmationWindow(WorkstationShopItemController shopItemController, string name, int price)
    {
        currentShopItemController = shopItemController;
        gameObject.SetActive(true);
        _confirmationText.text = "Purchase "+name+" for $"+price.ToString()+" ?";
    }
    public void YesButtonClicked()
    {
        currentShopItemController.Purchase();
        gameObject.SetActive(false);
    }
}
