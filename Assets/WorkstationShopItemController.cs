using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkstationShopItemController : MonoBehaviour
{
    private Image _image;
    private Text _name;
    private Text _price;
    private Text _numWorkers;
    private Image _buttonImage;
    public string m_name;
    public int m_price;
    public int m_numWorkers;
    public Sprite m_sprite;
    public WorkstationConstructionMasterController workstationMasterController;
    public WorkstationConfirmationWindowController confirmationWindowController;
    // Start is called before the first frame update
    void Start()
    {
        _image = gameObject.transform.Find("Image").GetComponent<Image>();
        _name = gameObject.transform.Find("Name").GetComponent<Text>();
        _price = gameObject.transform.Find("Price").GetComponent<Text>();
        _numWorkers = gameObject.transform.Find("NumWorkers").GetComponent<Text>();
        _buttonImage = gameObject.transform.Find("Purchase Button").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetWorkstationShopItem(string name, int price, int numWorkers, Sprite sprite)
    {
        m_name = name;
        m_price = price;
        m_numWorkers = numWorkers;
        m_sprite = sprite;
        _image.sprite = sprite;
        _name.text = name;
        _price.text = price.ToString();
        _numWorkers.text = "x" + numWorkers;
        ResetColour();
    }

    public void ResetColour()
    {
        if (m_price <= GameManager.instance.moneyOnHand)
        {
            _price.color = Color.green;
        }
        else
        {
            _price.color = Color.red;
            Color buttonImageColor = Color.grey;
            buttonImageColor.a = 0.30f;
            _buttonImage.color = buttonImageColor;
        }
    }

    public void Purchase()
    {
        // purchase action here
        // add to layout, decrease money, etc.
        GameManager.instance.LossMoney(m_price);
        workstationMasterController.ResetAllWorkstationItemControllerAfterPurchase();
    }

    public void ShowConfirmPurchaseWindow()
    {
        if (m_price <= GameManager.instance.moneyOnHand)
            confirmationWindowController.ShowConfirmationWindow(this, m_name, m_price);
    }
}
