using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkstationBuilder : MonoBehaviour
{
    public List<Workstation> workstations;
    public GameObject conveyor;
    public GameObject box;
    public float y_pos = -660f;
    public GameObject startSequence;
    public JobRequestMasterController jobRequestMasterController;
    private Dictionary<string, Workstation> workstationNameDict = new Dictionary<string, Workstation>();
    public bool startedProduction = false;
    public Image OnOffButton;
    public Sprite OnButtonSprite;
    public Sprite OffButtonSprite;
    public Text ProductionJobName;
    public Text ProductionJobStatus;

    public List<FactoryEntity> factoryEntities;

    [System.Serializable]
    public class Workstation
    {
        public string name;
        public GameObject prefab;
        public Sprite yellow;
        public Sprite brown;
        [HideInInspector]
        public float effective_width;
        public void Initialize()
        {
            effective_width = prefab.GetComponent<RectTransform>().rect.width;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        foreach (Workstation w in workstations)
        {
            w.Initialize();
            workstationNameDict.Add(w.name, w);
        }
        // saves to PlayerPrefs the current production job
        
        if (PlayerPrefs.HasKey("currentProductionJob"))
        {
            string currentisplayedName = jobRequestMasterController.GetDisplayedName(PlayerPrefs.GetString("currentProductionJob"));
            List<string> currentStations = jobRequestMasterController.GetWorkstationString(PlayerPrefs.GetString("currentProductionJob"));
            Build(currentisplayedName, currentStations);
            // generate workers
            for (int i = 0; i < PlayerPrefs.GetInt("currentProductionEnitiesNum"); i++)
            {
                //print(PlayerPrefs.GetInt("currentProductionEnityAssigned_" + i));
                if (PlayerPrefs.GetInt("currentProductionEnityAssigned_" + i) == 1)
                {
                    string colorStr = PlayerPrefs.GetString("currentProductionEnityColor_" + i);
                    string workerName = PlayerPrefs.GetString("currentProductionEnityWorkerName_" + i);
                    int stars = PlayerPrefs.GetInt("currentProductionEnityStar_" + i);
                    factoryEntities[i].AssignWorkerLoadTime(stars, colorStr, workerName);
                }
                else
                {

                }
            }
            if (PlayerPrefs.GetInt("ProductionRunning")==1)
            {
                StartAll();
            }
        }
        SetProductionJobStatusText();

        //Build(new List<string> {"saw","drill","drill"});
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Build(string display_name, List<string> names, float startin_pos = 500f)
    {
        EraseAll();
        factoryEntities = new List<FactoryEntity>();
        ProductionJobName.text = display_name;
        float current_x_pos = startin_pos;
        //// Create a box
        //GameObject bi = Instantiate(box, transform);
        //bi.GetComponent<RectTransform>().position = new Vector2(current_x_pos, y_pos);
        //current_x_pos += bi.GetComponent<RectTransform>().rect.width;
        string prev_n = names[0];
        foreach (string n in names)
        {
            if (prev_n != n)
            {
                // Create a conveyor
                GameObject conv = Instantiate(conveyor, transform);
                conv.GetComponent<RectTransform>().position = new Vector2(current_x_pos, y_pos);
                current_x_pos += conv.GetComponent<RectTransform>().rect.width;
            }
            // Create a workstation
            Workstation w = workstationNameDict[n];
            GameObject instance = Instantiate(w.prefab, transform);
            instance.GetComponent<RectTransform>().position = new Vector2(current_x_pos, y_pos);
            factoryEntities.Add(instance.GetComponent<FactoryEntity>());
            instance.GetComponent<FactoryEntity>().workstation_var_name = n;
            current_x_pos += w.effective_width;
            prev_n = n;
        }
        // Create a conveyor
        GameObject convf = Instantiate(conveyor, transform);
        convf.GetComponent<RectTransform>().position = new Vector2(current_x_pos, y_pos);
        current_x_pos += convf.GetComponent<RectTransform>().rect.width;
        //// Create a box
        //GameObject bo = Instantiate(box, transform);
        //bo.GetComponent<RectTransform>().position = new Vector2(current_x_pos, y_pos);
        //current_x_pos += bo.GetComponent<RectTransform>().rect.width;
        if (PlayerPrefs.GetInt("ProductionRunning") == 0)
        {
            StopAll();
        }
        SetProductionJobStatusText();
    }

    public float GetRequiredWidth(List<string> names)
    {
        float req_width = 0;
        string prev_n = names[0];
        //req_width += 2*box.GetComponent<RectTransform>().rect.width;
        foreach (string n in names)
        {
            if (prev_n != n)
            {
                req_width += conveyor.GetComponent<RectTransform>().rect.width;
            }
            Workstation w = workstationNameDict[n];
            req_width += w.effective_width;
        }
        req_width += conveyor.GetComponent<RectTransform>().rect.width;
        return req_width/1000f+1;
    }

    public void EraseAll()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void StartAll()
    {
        startedProduction = true;
        PlayerPrefs.SetInt("ProductionRunning", 1);
        OnOffButton.sprite = OffButtonSprite;
        
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<FactoryEntity>() != null)
            {
                child.gameObject.GetComponent<FactoryEntity>().StartProduction();
            };
        }
        foreach (Transform child in startSequence.transform)
        {
            if (child.gameObject.GetComponent<FactoryEntity>() != null)
            {
                child.gameObject.GetComponent<FactoryEntity>().StartProduction();
            };
        }
        print("Total star = "+ GetTotalStar().ToString());
        SetProductionJobStatusText();
    }

    public void StopAll()
    {
        startedProduction = false;
        PlayerPrefs.SetInt("ProductionRunning", 0);
        OnOffButton.sprite = OnButtonSprite;
        
        foreach (Transform child in transform)
        {
            if (child.gameObject.GetComponent<FactoryEntity>() != null)
            {
                child.gameObject.GetComponent<FactoryEntity>().StopProduction();
            };
        }
        foreach (Transform child in startSequence.transform)
        {
            if (child.gameObject.GetComponent<FactoryEntity>() != null)
            {
                child.gameObject.GetComponent<FactoryEntity>().StopProduction();
            };
        }
        SetProductionJobStatusText();
    }

    public void StartOrStopButtonClicked()
    {
        //TODO: check if all workers assigned
        bool allWorkerAssigned = CheckIfAllWorkerAssigned();
        if (!startedProduction)
        {
            if (allWorkerAssigned)
            {
                StartAll();
            }
            else
            {
                //TODO: print some error message to assign all workers first
            }
        }
        else
        {
            StopAll();
        }
    }

    public bool CheckIfAllWorkerAssigned()
    {
        bool checkIfAllWorkerAssigned = true;
        //Debug.Log(factoryEntities.Count);
        foreach (FactoryEntity f in factoryEntities)
        {
            if (!f.workerAssigned)
            {
                checkIfAllWorkerAssigned = false;
            }
        }
        if (factoryEntities.Count == 0)
        {
            return false;
        }
        return checkIfAllWorkerAssigned;
    }

    public bool CheckIfWorkerAlreadyAssigned(string name)
    {
        bool alreadyAssigned = false;
        foreach (FactoryEntity f in factoryEntities)
        {
            if (f.workerName==name)
            {
                alreadyAssigned = true;
            }
        }
        return alreadyAssigned;
    }

    public void SaveEntitiesToPlayerPrefs()
    {
        PlayerPrefs.SetInt("currentProductionEnitiesNum", factoryEntities.Count);
        for (int i=0; i< factoryEntities.Count; i++)
        {
            if (factoryEntities[i].workerAssigned)
            {
                PlayerPrefs.SetInt("currentProductionEnityAssigned_" + i, 1);
                PlayerPrefs.SetString("currentProductionEnityColor_" + i, factoryEntities[i].colorStr);
                PlayerPrefs.SetInt("currentProductionEnityStar_" + i, factoryEntities[i].stars);
                PlayerPrefs.SetString("currentProductionEnityWorkerName_" + i, factoryEntities[i].workerName);
            }
            else
            {
                PlayerPrefs.SetInt("currentProductionEnityAssigned_" + i, 0);
            }
            //print(PlayerPrefs.GetInt("currentProductionEnityAssigned_" + i));
        }
        SetProductionJobStatusText();
    }

    public void SetProductionJobStatusText()
    {
        if (startedProduction)
        {
            ProductionJobStatus.text = "||-----STARTED----||";
            ProductionJobStatus.color = Color.green;
            OnOffButton.gameObject.GetComponent<Button>().interactable = true;
        }
        else if (CheckIfAllWorkerAssigned())
        {
            ProductionJobStatus.text = "||------READY-----||";
            ProductionJobStatus.color = Color.black;
            OnOffButton.gameObject.GetComponent<Button>().interactable = true;
        }
        else if (factoryEntities.Count==0)
        {
            ProductionJobStatus.text = "||---CHOOSE JOB---||";
            ProductionJobStatus.color = Color.black;
            OnOffButton.gameObject.GetComponent<Button>().interactable = false;
        }
        else
        {
            ProductionJobStatus.text = "||---NEED WORKER--||";
            ProductionJobStatus.color = Color.red;
            OnOffButton.gameObject.GetComponent<Button>().interactable=false;
        }
    }

    public int GetTotalStar()
    {
        int total = 0;
        foreach (FactoryEntity f in factoryEntities)
        {
            total += f.stars;
        }
        return total;
    }
}
