using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkerShopConfirmationWindowController : MonoBehaviour
{
    private WorkerItemController currentShopItemController;
    public Text _confirmationText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowConfirmationWindow(WorkerItemController shopItemController, string name, int price)
    {
        currentShopItemController = shopItemController;
        gameObject.SetActive(true);
        _confirmationText.text = "Hire " + name + " for $" + price.ToString() + " ?";
    }
    public void YesButtonClicked()
    {
        currentShopItemController.Purchase();
        gameObject.SetActive(false);
    }
}
