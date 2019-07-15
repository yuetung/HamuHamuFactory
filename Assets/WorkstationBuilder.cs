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
    public JobRequestMasterController jobRequestMasterController;
    private Dictionary<string, Workstation> workstationNameDict = new Dictionary<string, Workstation>();

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
            List<string> currentStations = jobRequestMasterController.GetWorkstationString(PlayerPrefs.GetString("currentProductionJob"));
            Build(currentStations);
        }
        
        //Build(new List<string> {"saw","drill","drill"});
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Build(List<string> names, float startin_pos = 500f)
    {
        EraseAll();
        float current_x_pos = startin_pos;
        //// Create a box
        //GameObject bi = Instantiate(box, transform);
        //bi.GetComponent<RectTransform>().position = new Vector2(current_x_pos, y_pos);
        //current_x_pos += bi.GetComponent<RectTransform>().rect.width;
        foreach (string n in names)
        {
            // Create a workstation
            Workstation w = workstationNameDict[n];
            GameObject instance = Instantiate(w.prefab, transform);
            instance.GetComponent<RectTransform>().position = new Vector2(current_x_pos, y_pos);
            current_x_pos += w.effective_width;
            // Create a conveyor
            GameObject conv = Instantiate(conveyor, transform);
            conv.GetComponent<RectTransform>().position = new Vector2(current_x_pos, y_pos);
            current_x_pos += conv.GetComponent<RectTransform>().rect.width;
        }
        //// Create a box
        //GameObject bo = Instantiate(box, transform);
        //bo.GetComponent<RectTransform>().position = new Vector2(current_x_pos, y_pos);
        //current_x_pos += bo.GetComponent<RectTransform>().rect.width;
    }

    public float GetRequiredWidth(List<string> names)
    {
        float req_width = 0;
        //req_width += 2*box.GetComponent<RectTransform>().rect.width;
        foreach (string n in names)
        {
            Workstation w = workstationNameDict[n];
            req_width += w.effective_width;
            req_width += conveyor.GetComponent<RectTransform>().rect.width;
        }
        return req_width/1000f+1;
    }

    public void EraseAll()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
