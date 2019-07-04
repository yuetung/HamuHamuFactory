using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialShopItemController : MonoBehaviour
{
    private Image _image;
    private Text _name;
    private Text _price;
    public string m_display_name;
    public string m_variable_name;
    public int m_price;
    public Sprite m_sprite;
    public MaterialShopMasterController materialShopMasterController;
    public MaterialShopConfirmationWindowController confirmationWindowController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetMaterialShopItem(string display_name, string variable_name, int price, Sprite sprite)
    {
        _image = gameObject.transform.Find("Image").GetComponent<Image>();
        _name = gameObject.transform.Find("Name").GetComponent<Text>();
        _price = gameObject.transform.Find("Price").GetComponent<Text>();
        m_display_name = display_name;
        m_variable_name = variable_name;
        m_price = price;
        m_sprite = sprite;
        _image.sprite = sprite;
        _name.text = display_name;
        _price.text = price.ToString();
    }

    public void Purchase(int amount)
    {
        // purchase action here
        GameManager.instance.LossMoney(m_price*amount);
        GameManager.instance.GainMaterial(m_variable_name, amount, m_display_name);
    }

    public void ShowConfirmPurchaseWindow()
    {
        confirmationWindowController.ShowConfirmationWindow(this, m_display_name, m_price);
    }
}
