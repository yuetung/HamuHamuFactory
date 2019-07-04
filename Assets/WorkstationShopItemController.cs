using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkstationShopItemController : MonoBehaviour
{
    private Image _image;
    private Text _name;
    private Text _level;
    private Text _price;
    private Image _buttonImage;
    public string m_display_name;
    public string m_variable_name;
    public int m_level_requirement;
    public int m_price;
    public Sprite m_sprite;
    public WorkstationConstructionMasterController workstationMasterController;
    public WorkstationConfirmationWindowController confirmationWindowController;
    // Start is called before the first frame update
    void Start()
    {
        _image = gameObject.transform.Find("Image").GetComponent<Image>();
        _name = gameObject.transform.Find("Name").GetComponent<Text>();
        _level = gameObject.transform.Find("Level Req").GetComponent<Text>();
        _price = gameObject.transform.Find("Price").GetComponent<Text>();
        _buttonImage = gameObject.transform.Find("Purchase Button").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetWorkstationShopItem(string display_name, string variable_name, int level_requirement, int price, Sprite sprite)
    {
        m_display_name = display_name;
        m_variable_name = variable_name;
        m_level_requirement = level_requirement;
        m_price = price;
        m_sprite = sprite;
        _image.sprite = sprite;
        _name.text = display_name;
        _level.text = level_requirement.ToString();
        _price.text = price.ToString();
        ResetColour();
    }

    public void ResetColour()
    {
        bool p = false;
        bool l =false;

        if (m_price > GameManager.instance.moneyOnHand)
        {
            _price.color = Color.red;
            Color buttonImageColor = Color.grey;
            buttonImageColor.a = 0.30f;
            _buttonImage.color = buttonImageColor;
        }
        else
        {
            _price.color = Color.green;
            p = true;
        }
        if (m_level_requirement > GameManager.instance.currentLevel)
        {
            _level.color = Color.red;
            Color buttonImageColor = Color.grey;
            buttonImageColor.a = 0.30f;
            _buttonImage.color = buttonImageColor;
        }
        else
        {
            _level.color = Color.green;
            l = true;
        }
        if (p && l)
        {
            Color buttonImageColor = Color.grey;
            buttonImageColor.a = 0.0f;
            _buttonImage.color = buttonImageColor;
        }
    }

    public void Purchase()
    {
        // purchase action here
        GameManager.instance.LossMoney(m_price);
        GameManager.instance.GainWorkstation(m_variable_name,m_display_name);
        workstationMasterController.ResetAllWorkstationItemControllerAfterPurchase();
    }

    public void ShowConfirmPurchaseWindow()
    {
        if (m_price <= GameManager.instance.moneyOnHand && m_level_requirement <= GameManager.instance.currentLevel)
            confirmationWindowController.ShowConfirmationWindow(this, m_display_name, m_price);
    }
}
