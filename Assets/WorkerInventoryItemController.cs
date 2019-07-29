using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkerInventoryItemController : MonoBehaviour
{
    private Image _image;
    private Text _name;
    private Text _proficiency0;
    private List<GameObject> _stars0;
    private Text _proficiency1;
    private List<GameObject> _stars1;
    private GameObject assignImage;
    public string m_name;
    private string mode;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetWorker(string mode, string name, string colorStr, int spriteNum, List<string> workstations, List<int> workstationStats)
    {
        this.mode = mode;
        _image = gameObject.transform.Find("Image").GetComponent<Image>();
        _name = gameObject.transform.Find("Name").GetComponent<Text>();
        m_name = name;
        _proficiency0 = gameObject.transform.Find("Proficiency1").GetComponent<Text>();
        _proficiency1 = gameObject.transform.Find("Proficiency2").GetComponent<Text>();
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
        assignImage = gameObject.transform.Find("Assign img").gameObject;
        assignImage.SetActive(false);
        _name.text = name;
        _image.sprite = WorkerShopMasterController.sprites[spriteNum];
        _image.color = ToColor(colorStr);
        _proficiency0.text = workstations[0];
        _proficiency1.text = workstations[1];
        for (int i = 0; i < _stars0.Count; i++)
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
    }

    public void PointerEnter()
    {
        if (mode == "inspect")
        {
            //assignImage.GetComponent<Image>().color = Color.red;
            //assignImage.transform.Find("Name").gameObject.GetComponent<Text>().text = "Fire";
            //assignImage.SetActive(true);
        }
        else if (mode == "assign")
        {
            // Check if already assigned or no matching skill
            bool assignedAlready = true;
            bool noMatchingSkill = false;
            if (assignedAlready)
            {
                assignImage.GetComponent<Image>().color = Color.gray;
                assignImage.transform.Find("Name").gameObject.GetComponent<Text>().text = "Already Assiged";
                assignImage.SetActive(true);
            }
            else if (noMatchingSkill)
            {
                assignImage.GetComponent<Image>().color = Color.gray;
                assignImage.transform.Find("Name").gameObject.GetComponent<Text>().text = "No Matching Skill";
                assignImage.SetActive(true);
            }
        }
        
    }

    public void PointerExit()
    {
        assignImage.SetActive(false);
    }
    public void Click()
    {
        Debug.Log(m_name + " clicked");
        if (mode == "inspect")
        {
            //Debug.Log("TODO: showing fire confirmation window");
            //TODO: show fire confirmation window and fire the guy
        }
        else if (mode == "assign")
        {
            // TODO: Check if already assigned or no matching skill
            bool assignedAlready = true;
            bool noMatchingSkill = false;
            if (!assignedAlready && !noMatchingSkill)
            {
                Debug.Log("TODO: assigning to workstation");
                // TODO: assign to workstation here
            }
        }
    }
    //Utilities
    public Color ToColor(string color)
    {
        return (Color)typeof(Color).GetProperty(color.ToLowerInvariant()).GetValue(null, null);
    }
}
