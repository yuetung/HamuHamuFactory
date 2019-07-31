using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkerInventoryItemController : MonoBehaviour
{
    private Image _image;
    private Text _name;
    private List<Text> _proficiencies = new List<Text>();
    private List<GameObject> _stars0;
    private List<GameObject> _stars1;
    private GameObject assignImage;
    
    private string mode;
    private string m_name;
    private string m_colorStr;
    private int m_spriteNum;
    private List<string> m_workstations;
    private List<int> m_workstationStats;
    private FactoryEntity factoryEntity = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetWorker(string mode, string name, string colorStr, int spriteNum, List<string> workstations, List<int> workstationStats,
        FactoryEntity factoryEntity=null)
    {
        this.mode = mode;
        if (mode == "assign")
        {
            this.factoryEntity = factoryEntity;
        }
        _image = gameObject.transform.Find("Image").GetComponent<Image>();
        _name = gameObject.transform.Find("Name").GetComponent<Text>();
        m_name = name;
        m_colorStr = colorStr;
        m_spriteNum = spriteNum;
        m_workstations = workstations;
        m_workstationStats = workstationStats;

        _proficiencies.Add(gameObject.transform.Find("Proficiency1").GetComponent<Text>());
        _proficiencies.Add(gameObject.transform.Find("Proficiency2").GetComponent<Text>());
        _stars0 = new List<GameObject>
        {
            _proficiencies[0].gameObject.transform.Find("Star img").gameObject,
            _proficiencies[0].gameObject.transform.Find("Star img (1)").gameObject,
            _proficiencies[0].gameObject.transform.Find("Star img (2)").gameObject
        };
        _stars1 = new List<GameObject>
        {
            _proficiencies[1].gameObject.transform.Find("Star img").gameObject,
            _proficiencies[1].gameObject.transform.Find("Star img (1)").gameObject,
            _proficiencies[1].gameObject.transform.Find("Star img (2)").gameObject
        };
        assignImage = gameObject.transform.Find("Assign img").gameObject;
        assignImage.SetActive(false);
        _name.text = name;
        _image.sprite = WorkerShopMasterController.sprites[spriteNum];
        _image.color = ToColor(colorStr);
        _proficiencies[0].text = workstations[0];
        _proficiencies[1].text = workstations[1];
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
        if (mode == "assign")
        {
            HighlightProficiencyText();
        }
        else if (mode == "inspect")
        {
            DehighlightProficiencyText();
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
            bool assignedAlready = factoryEntity.workstationBuilder.CheckIfWorkerAlreadyAssigned(m_name);
            int matchingSkillNum = GetMatchingSkillNum(); //0 = no matching skill, 1 = match skill 1, 2 = match skill 2
            if (assignedAlready)
            {
                assignImage.GetComponent<Image>().color = Color.gray;
                assignImage.transform.Find("Name").gameObject.GetComponent<Text>().text = "Already Assiged";
                assignImage.SetActive(true);
            }
            else if (matchingSkillNum==0)
            {
                assignImage.GetComponent<Image>().color = Color.gray;
                assignImage.transform.Find("Name").gameObject.GetComponent<Text>().text = "No Matching Skill";
                assignImage.SetActive(true);
            }
            else
            {
                assignImage.GetComponent<Image>().color = Color.green;
                assignImage.transform.Find("Name").gameObject.GetComponent<Text>().text = "Assign";
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
        //Debug.Log(m_name + " clicked");
        if (mode == "inspect")
        {
            //Debug.Log("TODO: showing fire confirmation window");
            //TODO: show fire confirmation window and fire the guy
        }
        else if (mode == "assign")
        {
            // TODO: Check if already assigned or no matching skill
            bool assignedAlready = factoryEntity.workstationBuilder.CheckIfWorkerAlreadyAssigned(m_name);
            int matchingSkillNum = GetMatchingSkillNum(); //0 = no matching skill, 1 = match skill 1, 2 = match skill 2
            if (!assignedAlready && matchingSkillNum != 0)
            {
                factoryEntity.AssignWorker(m_workstationStats[matchingSkillNum-1], m_colorStr, m_name);
                gameObject.transform.parent.gameObject.SetActive(false);
            }
        }
    }

    private int GetMatchingSkillNum()
    {
        for (int i=0; i<m_workstations.Count; i++)
        {
            if (m_workstations[i] == factoryEntity.workstation_var_name)
            {
                return i+1;
            }
        }
        return 0;
    }

    public void HighlightProficiencyText()
    {
        for (int i = 0; i < _proficiencies.Count; i++)
        {
            _proficiencies[i].fontStyle = FontStyle.Normal;
            _proficiencies[i].color = Color.gray;
        }
        int matchingSkillNum = GetMatchingSkillNum();
        if (matchingSkillNum != 0)
        {
            _proficiencies[matchingSkillNum - 1].fontStyle = FontStyle.Bold;
            _proficiencies[matchingSkillNum - 1].color = Color.green;
        }
    }

    public void DehighlightProficiencyText()
    {
        for (int i = 0; i < _proficiencies.Count; i++)
        {
            _proficiencies[i].fontStyle = FontStyle.Normal;
            _proficiencies[i].color = Color.black;
        }
    }
    //Utilities
    public Color ToColor(string color)
    {
        return (Color)typeof(Color).GetProperty(color.ToLowerInvariant()).GetValue(null, null);
    }
}
