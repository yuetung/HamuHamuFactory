using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemController : MonoBehaviour
{
    private Image _image;
    public Text _name;
    private Text _amount;
    private GameObject _name_background;
    public string m_display_name;
    public string m_variable_name;
    public Sprite m_sprite;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetShopItem(string display_name, string variable_name, Sprite sprite)
    {
        _image = gameObject.transform.Find("Image").GetComponent<Image>();
        _amount = gameObject.transform.Find("Amount").GetComponent<Text>();
        _name_background = gameObject.transform.Find("NameBackground").gameObject;
        Debug.Log(_name_background.ToString());
        m_display_name = display_name;
        m_variable_name = variable_name;
        m_sprite = sprite;
        _image.sprite = sprite;
        _name.text = display_name;
        _amount.text = "x "+PlayerPrefs.GetInt(variable_name).ToString();
        _name_background.SetActive(false);
    }

    public void ShowName()
    {
        _name_background.SetActive(true);
    }

    public void DisableName()
    {
        _name_background.SetActive(false);
    }

}
