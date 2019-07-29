using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkerItemController : MonoBehaviour
{
    private Image _image;
    private Text _name;
    private Text _price;
    private Text _tier;
    private Text _proficiency0;
    private List<GameObject> _stars0;
    private Text _proficiency1;
    private List<GameObject> _stars1;
    public string m_name;
    public int m_base_price;
    public int m_tier;
    public Color m_color;
    public Sprite m_sprite;
    public List<string> m_workstations;
    public List<int> m_workstationStats;
    private Image _buttonImage;
    public WorkerShopMasterController workerShopMasterController;
    public WorkerShopConfirmationWindowController confirmationWindowController;
    private int worker_index;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetWorkerItem(int worker_index, string name, int base_price, int tier, Color color, Sprite sprite, List<string> workstations, List<int> workstationStats)
    {
        this.worker_index = worker_index;
        _image = gameObject.transform.Find("Image").GetComponent<Image>();
        _name = gameObject.transform.Find("Name").GetComponent<Text>();
        _price = gameObject.transform.Find("Price").GetComponent<Text>();
        _tier = gameObject.transform.Find("Tier").GetComponent<Text>();
        _proficiency0 = gameObject.transform.Find("Proficiency1").GetComponent<Text>();
        _proficiency1 = gameObject.transform.Find("Proficiency2").GetComponent<Text>();
        _buttonImage = gameObject.transform.Find("Purchase Button").GetComponent<Image>();
        _stars0 = new List<GameObject>
        {
            _proficiency0.gameObject.transform.Find("Star img").gameObject,
            _proficiency0.gameObject.transform.Find("Star img (1)").gameObject,
            _proficiency0.gameObject.transform.Find("Star img (2)").gameObject
        };
        _stars1 = new List<GameObject>
        {
            _proficiency1.gameObject.transform.Find("Star img").gameObject,
            _proficiency1.gameObject.transform.Find("Star img (1)").gameObject,
            _proficiency1.gameObject.transform.Find("Star img (2)").gameObject
        };
        m_name = name;
        m_base_price = base_price;
        m_sprite = sprite;
        m_tier = tier;
        m_color = color;
        m_sprite = sprite;
        m_workstations = workstations;
        m_workstationStats = workstationStats;
        _image.sprite = sprite;
        _image.color = color;
        _name.text = name;
        _price.text = base_price.ToString();
        switch (tier)
        {
            case 0:
                _tier.text = "Average";
                _tier.color = Color.black;
                break;
            case 1:
                _tier.text = "Good";
                _tier.color = Color.red;
                break;
            case 2:
                _tier.text = "Rare";
                _tier.color = Color.blue;
                break;
            case 3:
                _tier.text = "Very Rare";
                _tier.color = Color.magenta;
                break;
            case 4:
                _tier.text = "Legendary";
                _tier.color = Color.green;
                break;
        }
        _proficiency0.text = workstations[0];
        _proficiency1.text = workstations[1];
        for (int i=0; i< _stars0.Count; i++)
        {
            _stars0[i].SetActive(false);
            _stars1[i].SetActive(false);
        }
        for (int i = 0; i < workstationStats[0]; i++)
        {
            _stars0[i].SetActive(true);
        }
        for (int i = 0; i < workstationStats[1]; i++)
        {
            _stars1[i].SetActive(true);
        }
        ResetColour();
    }

    public void Purchase()
    {
        // purchase action here
        GameManager.instance.LossMoney(m_base_price);
        //GameManager.instance.GainMaterial(m_variable_name, amount, m_display_name);
        //TODO: remove from playerpref and refresh here
        workerShopMasterController.RemoveWorkerFromShop(worker_index);
    }

    public void ResetColour()
    {
        if (m_base_price > GameManager.instance.moneyOnHand)
        {
            _price.color = Color.red;
            Color buttonImageColor = Color.grey;
            buttonImageColor.a = 0.30f;
            _buttonImage.color = buttonImageColor;
        }
        else
        {
            _price.color = Color.green;
            Color buttonImageColor = Color.grey;
            buttonImageColor.a = 0.0f;
            _buttonImage.color = buttonImageColor;
        }
    }

    public void ShowConfirmPurchaseWindow()
    {
        if (m_base_price <= GameManager.instance.moneyOnHand)
            confirmationWindowController.ShowConfirmationWindow(this, m_name, m_base_price);
    }
}
