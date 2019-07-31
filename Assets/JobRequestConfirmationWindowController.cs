using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JobRequestConfirmationWindowController : MonoBehaviour
{
    private JobRequestItemController currentShopItemController;
    public Text _confirmationText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowConfirmationWindow(JobRequestItemController shopItemController, string name)
    {
        currentShopItemController = shopItemController;
        gameObject.SetActive(true);
        _confirmationText.text = "Start producing " + name + " ?";
    }
    public void YesButtonClicked()
    {
        currentShopItemController.StartBuilding();
        gameObject.SetActive(false);
        gameObject.transform.parent.gameObject.SetActive(false);
    }
}
