using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobRequestMasterController : MonoBehaviour
{
    public JobRequestItemController[] itemControllers;

    [System.Serializable]
    public class JobItemEntry
    {
        public string display_name;
        public string variable_name;
        public int unit_reward;
        public Sprite sprite;
        public int production_time;
        public string[] workstation_names;
        public int[] workstation_nums;
        public string[] material_names;
        public int[] material_nums;
    }
    public JobItemEntry[] jobItemEntries;
    private List<JobItemEntry> availableJobItems;
    public Dictionary<string, JobItemEntry> var_name_to_item_entry;
    private bool started = false;

    private void Awake()
    {
        var_name_to_item_entry = new Dictionary<string, JobItemEntry>();
        for (int i = 0; i < jobItemEntries.Length; i++)
        {
            var_name_to_item_entry[jobItemEntries[i].variable_name] = jobItemEntries[i];
        }
    }

    private void OnEnable()
    {
        if (started)
            ShowAvailableJobItems();
    }

    // Start is called before the first frame update
    void Start()
    {
        ShowAvailableJobItems();
        started = true;
        gameObject.SetActive(false);
        GameManager.instance.GainProductionJob("wooden_table", "Wooden Table");
        GameManager.instance.GainProductionJob("wooden_table_L", "Big Wooden Table");
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    // The last 3 workstation shop items varies daily
    public void ShowAvailableJobItems()
    {
        availableJobItems = new List<JobItemEntry>();
        // retrieve list of available job items from playerprefs
        foreach (string var_name in var_name_to_item_entry.Keys)
        {
            if (PlayerPrefs.HasKey(var_name))
            {
                availableJobItems.Add(var_name_to_item_entry[var_name]);
            }
        }
        // hide all itemControllers not in-use
        for (int i = availableJobItems.Count; i < itemControllers.Length; i++)
        {
            itemControllers[i].gameObject.SetActive(false);
        }
        // first few item are fixed
        for (int i = 0; i < availableJobItems.Count; i++)
        {
            itemControllers[i].gameObject.SetActive(true);
            itemControllers[i].SetJobItem(availableJobItems[i].display_name, availableJobItems[i].variable_name, availableJobItems[i].unit_reward,
                availableJobItems[i].sprite, availableJobItems[i].production_time, availableJobItems[i].workstation_names,
                availableJobItems[i].workstation_nums, availableJobItems[i].material_names, availableJobItems[i].material_nums);
        }
    }

    //public void ResetAllJobItemColours()
    //{
    //    for (int i = 0; i < availableJobItems.Length; i++)
    //    {
    //        itemControllers[i].ResetColour();
    //    }
    //}

    public List<string> GetWorkstationString(string name)
    {
        print(name);
        JobItemEntry job = var_name_to_item_entry[name];
        string[] workstation_names = job.workstation_names;
        int[] workstation_nums = job.workstation_nums;
        List<string> w = new List<string>();
        for (int i = 0; i < workstation_names.Length; i++)
        {
            for (int j = 0; j < workstation_nums[i]; j++)
            {
                w.Add(workstation_names[i]);
            }
        }
        return w;
    }
}
