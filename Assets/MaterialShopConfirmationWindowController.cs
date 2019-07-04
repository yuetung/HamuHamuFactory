using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialShopConfirmationWindowController : MonoBehaviour
{
    private MaterialShopItemController currentShopItemController;
    public Text _confirmationText;
    public List<int> bundle_amounts;
    public List<Button> buttons;
    private int m_price;
    private bool started = false;

    private void OnEnable()
    {
        if (started)
            ResetColour();
    }

    // Start is called before the first frame update
    void Start()
    {
        ResetColour();
        started = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ShowConfirmationWindow(MaterialShopItemController shopItemController, string name, int price)
    {
        m_price = price;
        currentShopItemController = shopItemController;
        gameObject.SetActive(true);
        _confirmationText.text = "Purchase how many " + name + " ?";
    }
    public void ButtonClicked(int amount)
    {
        if (m_price * amount <= GameManager.instance.moneyOnHand)
        {
            currentShopItemController.Purchase(amount);
            gameObject.SetActive(false);
        }
    }
    public void ResetColour()
    {
        for (int i=0; i<buttons.Count; i++)
        {
            if (m_price*bundle_amounts[i] > GameManager.instance.moneyOnHand)
            {
                buttons[i].interactable = false;

            }
            else
            {
                buttons[i].interactable = true;
            }
        }
        
    }
}
