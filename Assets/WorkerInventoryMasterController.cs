using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerInventoryMasterController : MonoBehaviour
{
    public WorkerShopMasterController workerShopMaster;
    public List<WorkerInventoryItemController> inv_workers;
    public string mode = "inspect";

    public void OnEnable()
    {

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenWorkerInventory(string mode = "inspect")
    {
        if (mode == "inspect")
        {
            this.mode = mode;
            gameObject.SetActive(true);
            DisplayInventory();
        }
        else if (mode == "assign")
        {
            this.mode = mode;
            gameObject.SetActive(true);
            DisplayInventory();
        }
    }

    public void DisplayInventory()
    {
        // Disables undisplayed workstations/ materials
        for (int i = 0; i < inv_workers.Count; i++)
        {
            inv_workers[i].gameObject.SetActive(false);
        }
        // set all workstations available to show
        if (PlayerPrefs.HasKey("TotalNumWorkers"))
        {
            int numWorkers = PlayerPrefs.GetInt("TotalNumWorkers");
            for (int i = 0; i < Mathf.Min(numWorkers, inv_workers.Count); i++)
            {
                inv_workers[i].gameObject.SetActive(true);
                string name = PlayerPrefs.GetString("workerName_" + i);
                string colorStr = PlayerPrefs.GetString("workerColorStr_" + i);
                int spriteNum = PlayerPrefs.GetInt("workerSpriteNum_" + i);
                string workstations_0 = PlayerPrefs.GetString("workerProficiency0_" + i);
                string workstations_1 = PlayerPrefs.GetString("workerProficiency1_" + i);
                int workstationStats_0 = PlayerPrefs.GetInt("workerProficiencyStat0_" + i);
                int workstationStats_1 = PlayerPrefs.GetInt("workerProficiencyStat1_" + i);
                inv_workers[i].SetWorker(mode, name, colorStr, spriteNum, new List<string> { workstations_0, workstations_1 },
                    new List<int> { workstationStats_0, workstationStats_1 });
            }
        }
    }
}
