using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JobRequestItemController : MonoBehaviour
{
    private Image _image;
    private Text name_text;
    private Text reward_text;
    private Text production_time_text;
    public Image[] workstation_images;
    public Text[] workstation_nums_texts;
    public Image[] material_images;
    public Text[] material_nums_texts;
    private Image _buttonImage;
    private Text required_length_text;

    private string m_display_name;
    private string m_variable_name;
    private int m_reward;
    private Sprite m_sprite;
    private int m_production_time;
    private string[] m_workstation_names;
    private int[] m_workstation_nums;
    private string[] m_material_names;
    private int[] m_material_nums;
    private int m_required_length;
    public MaterialShopMasterController materialShopMasterController;
    public WorkstationConstructionMasterController workstationShopMasterController;
    public JobRequestMasterController jobRequestMasterController;
    public JobRequestConfirmationWindowController confirmationWindowController;
    public WorkstationBuilder workstationBuilder;

    private bool locked = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetJobItem(string display_name, string variable_name, int reward, Sprite sprite, int production_time, string[] workstation_names, 
        int[] workstation_nums, string[] material_names, int[] material_nums)
    {
        _image = gameObject.transform.Find("Image").GetComponent<Image>();
        name_text = gameObject.transform.Find("Name").GetComponent<Text>();
        reward_text = gameObject.transform.Find("reward").GetComponent<Text>();
        production_time_text = gameObject.transform.Find("time required").GetComponent<Text>();
        _buttonImage = gameObject.transform.Find("Purchase Button").GetComponent<Image>();
        required_length_text = gameObject.transform.Find("meters required").GetComponent<Text>();
        m_display_name = display_name;
        m_variable_name = variable_name;
        m_reward = reward;
        m_sprite = sprite;
        m_production_time = production_time;
        m_workstation_names = workstation_names;
        m_workstation_nums = workstation_nums;
        m_material_names = material_names;
        m_material_nums = material_nums;
        m_required_length = Mathf.CeilToInt(workstationBuilder.GetRequiredWidth(GetWorkstationString()));

        _image.sprite = sprite;
        name_text.text = display_name;
        reward_text.text = reward.ToString();
        required_length_text.text = m_required_length.ToString();
        production_time_text.text = production_time.ToString();

        for (int i = workstation_names.Length; i < workstation_images.Length; i++)
        {
            workstation_images[i].gameObject.SetActive(false);
            workstation_nums_texts[i].gameObject.SetActive(false);
        }

        for (int i=0; i<workstation_names.Length; i++)
        {
            workstation_images[i].gameObject.SetActive(true);
            workstation_nums_texts[i].gameObject.SetActive(true);
            workstation_images[i].sprite = workstationShopMasterController.var_name_to_item_entry[workstation_names[i]].sprite;
            workstation_nums_texts[i].text = workstation_nums[i].ToString();
        }

        for (int i = material_names.Length; i < material_images.Length; i++)
        {
            material_images[i].gameObject.SetActive(false);
            material_nums_texts[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < material_names.Length; i++)
        {
            material_images[i].gameObject.SetActive(true);
            material_nums_texts[i].gameObject.SetActive(true);
            material_images[i].sprite = materialShopMasterController.var_name_to_item_entry[material_names[i]].sprite;
            material_nums_texts[i].text = material_nums[i].ToString();
        }

        ResetColour();
    }

    public void ResetColour()
    {
        locked = false;
        for (int i=0; i<m_workstation_names.Length; i++)
        {
            if (PlayerPrefs.HasKey(m_workstation_names[i]))
            {
                int inv_amount = PlayerPrefs.GetInt(m_workstation_names[i]);
                if (inv_amount < m_workstation_nums[i]) // not enough
                {
                    workstation_nums_texts[i].color = Color.red;
                    locked = true;
                }
                else
                {
                    workstation_nums_texts[i].color = Color.green;
                }
            }
            else // don't have
            {
                workstation_nums_texts[i].color = Color.red;
                locked = true;
            }
        }
        if (m_required_length > PlayerPrefs.GetInt("roomSize"))
        {
            required_length_text.color = Color.red;
            locked = true;
        }
        else
        {
            required_length_text.color = Color.green;
        }
        if (locked)
        {
            Color buttonImageColor = Color.grey;
            buttonImageColor.a = 0.30f;
            _buttonImage.color = buttonImageColor;
        }
        else
        {
            Color buttonImageColor = Color.grey;
            buttonImageColor.a = 0.0f;
            _buttonImage.color = buttonImageColor;
        }
        
    }

    public void StartBuilding()
    {
        if (!locked)
        {
            print("building line");
            PlayerPrefs.SetString("currentProductionJob", m_variable_name);
            PlayerPrefs.SetInt("ProductionRunning", 0);
            workstationBuilder.Build(m_display_name,GetWorkstationString());
            workstationBuilder.SaveEntitiesToPlayerPrefs();
        }
        // purchase action here
        //GameManager.instance.LossMoney(m_price * amount);
        //GameManager.instance.GainMaterial(m_variable_name, amount, m_display_name);
    }

    public void ShowConfirmPurchaseWindow()
    {
        if (!locked)
            confirmationWindowController.ShowConfirmationWindow(this, m_display_name);
    }

    public List<string> GetWorkstationString()
    {
        List<string> w = new List<string>();
        for (int i = 0; i < m_workstation_names.Length; i++)
        {
            for (int j = 0; j < m_workstation_nums[i]; j++)
            {
                w.Add(m_workstation_names[i]);
            }
        }
        return w;
    }
}
