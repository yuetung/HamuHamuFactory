using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkstationBuilder : MonoBehaviour
{
    public List<Workstation> workstations;
    public GameObject conveyor;
    public GameObject teleporter;
    public GameObject producePrefab;
    public float y_pos = -660f;
    public GameObject startSequence;
    public JobRequestMasterController jobRequestMasterController;
    public MaterialShopMasterController materialShopMasterController;
    private Dictionary<string, Workstation> workstationNameDict = new Dictionary<string, Workstation>();
    public bool startedProduction = false;
    public Image OnOffButton;
    public Sprite OnButtonSprite;
    public Sprite OffButtonSprite;
    public Text ProductionJobName;
    public Text ProductionJobStatus;
    public Image[] materialImages;
    public Text[] materialTexts;
    private List<float> produceWorkstationPositions = new List<float>();
    private float produce_end_x = 100f;
    private Sprite[] produceSprites;
    private List<GameObject> currentProduces = new List<GameObject>();
    private TeleporterController teleporterController;

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
        SetMaterialText();

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
        List<float> workstationPositions = new List<float>();
        workstationPositions.Add(current_x_pos); // workstation start
        string prev_n = names[0];
        foreach (string n in names)
        {
            if (prev_n != n)
            {
                workstationPositions.Add(current_x_pos); // conveyor start
                // Create a conveyor
                GameObject conv = Instantiate(conveyor, transform);
                conv.GetComponent<RectTransform>().position = new Vector2(current_x_pos, y_pos);
                current_x_pos += conv.GetComponent<RectTransform>().rect.width;
                workstationPositions.Add(current_x_pos); // conveyor end
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
        workstationPositions.Add(current_x_pos); // workstation end
        // Create a conveyor
        GameObject convf = Instantiate(conveyor, transform);
        convf.GetComponent<RectTransform>().position = new Vector2(current_x_pos, y_pos);
        current_x_pos += convf.GetComponent<RectTransform>().rect.width;
        float end_x = current_x_pos+130; // final conveyor end
        // Create a teleporter
        GameObject tele = Instantiate(teleporter, transform);
        tele.GetComponent<RectTransform>().position = new Vector2(current_x_pos, y_pos);
        current_x_pos += tele.GetComponent<RectTransform>().rect.width;
        teleporterController = tele.GetComponent<TeleporterController>();
        if (PlayerPrefs.GetInt("ProductionRunning") == 0)
        {
            StopAll();
        }
        SetProductionJobStatusText();
        SetProduceParams(workstationPositions,end_x);
        SetMaterialText();
        DeleteAllCurrentProduces();
    }

    public float GetRequiredWidth(List<string> names)
    {
        float req_width = 0;
        string prev_n = names[0];
        req_width += teleporter.GetComponent<RectTransform>().rect.width;
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
        InvokeRepeating("Produce", 3f, jobRequestMasterController.GetProductionTime(PlayerPrefs.GetString("currentProductionJob")));
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
        CancelInvoke();
        DeleteAllCurrentProduces();
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
        if (!startedProduction)
        {
            if (CheckIfAllWorkerAssigned() && CheckSufficientMaterial())
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

    public bool CheckSufficientMaterial()
    {
        bool enoughMaterial = true;
        string[] materialNames = jobRequestMasterController.GetMaterialNames(PlayerPrefs.GetString("currentProductionJob"));
        int[] materialNums = jobRequestMasterController.GetMaterialNums(PlayerPrefs.GetString("currentProductionJob"));
        for (int i=0; i<materialNames.Length; i++)
        {
            if (!PlayerPrefs.HasKey(materialNames[i]) || PlayerPrefs.GetInt(materialNames[i])< materialNums[i])
            {
                enoughMaterial = false;
            }
        }
        return enoughMaterial;
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
        else if (CheckIfAllWorkerAssigned() && CheckSufficientMaterial())
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
        else if (!CheckIfAllWorkerAssigned())
        {
            ProductionJobStatus.text = "||---NEED WORKER--||";
            ProductionJobStatus.color = Color.red;
            OnOffButton.gameObject.GetComponent<Button>().interactable=false;
        }
        else if (!CheckSufficientMaterial())
        {
            ProductionJobStatus.text = "||--NEED MATERIAL-||";
            ProductionJobStatus.color = Color.red;
            OnOffButton.gameObject.GetComponent<Button>().interactable = false;
        }
    }

    public void SetMaterialText()
    {
        if (PlayerPrefs.HasKey("currentProductionJob"))
        {
            string[] materialNames = jobRequestMasterController.GetMaterialNames(PlayerPrefs.GetString("currentProductionJob"));
            //int[] materialNums = jobRequestMasterController.GetMaterialNums(PlayerPrefs.GetString("currentProductionJob"));
            for (int i = 0; i < materialImages.Length; i++)
            {
                materialImages[i].gameObject.SetActive(false);
                materialTexts[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < materialNames.Length; i++)
            {
                materialImages[i].gameObject.SetActive(true);
                materialTexts[i].gameObject.SetActive(true);
                materialImages[i].overrideSprite = materialShopMasterController.GetMaterialSprite(materialNames[i]);
                if (!PlayerPrefs.HasKey(materialNames[i]))
                {
                    materialTexts[i].text = "x0";
                }
                else
                {
                    materialTexts[i].text = "x" + PlayerPrefs.GetInt(materialNames[i]).ToString();
                }
            }
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

    public void SetProduceParams(List<float> workstationPositions, float end_x)
    {
        produceWorkstationPositions = workstationPositions;
        produce_end_x = end_x;
    }

    public void Produce()
    {
        if (CheckSufficientMaterial())
        {
            string[] materialNames = jobRequestMasterController.GetMaterialNames(PlayerPrefs.GetString("currentProductionJob"));
            int[] materialNums = jobRequestMasterController.GetMaterialNums(PlayerPrefs.GetString("currentProductionJob"));
            for (int i = 0; i < materialNames.Length; i++)
            {
                GameManager.instance.LossMaterial(materialNames[i], materialNums[i]);
            }
            SetMaterialText();
            GameObject currentProduce = Instantiate(producePrefab, new Vector2(-120f, y_pos + 180), Quaternion.identity, transform);
            currentProduce.GetComponent<ProduceController>().Initiate(this, produceWorkstationPositions, jobRequestMasterController.GetProductionSprites(PlayerPrefs.GetString("currentProductionJob")),
                produce_end_x, jobRequestMasterController.GetProductionTime(PlayerPrefs.GetString("currentProductionJob"))*3,
                jobRequestMasterController.GetUnitReward(PlayerPrefs.GetString("currentProductionJob")));
            currentProduces.Add(currentProduce);
        }
        else
        {
            Invoke("StopAll", jobRequestMasterController.GetProductionTime(PlayerPrefs.GetString("currentProductionJob"))*3);
        }
    }

    public void DeleteAllCurrentProduces()
    {
        foreach (GameObject p in currentProduces)
        {
            // gain back material if object still around
            if (p!=null)
            {
                string[] materialNames = jobRequestMasterController.GetMaterialNames(PlayerPrefs.GetString("currentProductionJob"));
                int[] materialNums = jobRequestMasterController.GetMaterialNums(PlayerPrefs.GetString("currentProductionJob"));
                for (int i = 0; i < materialNames.Length; i++)
                {
                    GameManager.instance.GainMaterial(materialNames[i], materialNums[i]);
                }
            }
            Destroy(p);
        }
        currentProduces = new List<GameObject>();
    }

    public void Teleport()
    {
        teleporterController.Teleport();
    }
    public void OnApplicationQuit()
    {
        DeleteAllCurrentProduces();
    }
}
