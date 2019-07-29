using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerShopMasterController : MonoBehaviour
{
    public WorkerItemController[] itemControllers;
    public static Sprite[] sprites;
    public Sprite[] init_sprites;

    [System.Serializable]
    public class WorkerEntry
    {
        public string name;
        public int base_price;
        public int tier;  // 0 for normal, 4 for legendary
        public string colorStr;
        public Color color;
        public int spriteNum;
        public Sprite sprite;
        public List<string> workstations;
        public List<int> workstationStats;
        public WorkerEntry(string name, int base_price, int tier, string colorStr, int spriteNum, List<string> workstations, List<int> workstationStats)
        {
            this.name = name;
            this.base_price = base_price;
            this.tier = tier;
            this.colorStr = colorStr;
            color = ToColor(colorStr);
            this.spriteNum = spriteNum;
            sprite = sprites[spriteNum];
            this.workstations = workstations;
            this.workstationStats = workstationStats;
        }
        //Utilities
        public Color ToColor(string color)
        {
            return (Color)typeof(Color).GetProperty(color.ToLowerInvariant()).GetValue(null, null);
        }
    }
    public List<WorkerEntry> todayWorkerEntries;
    private int slots = 2;
    //public ShopItemEntry[] variableShopItemEntries;
    private bool started = false;

    private void Awake()
    {
        if (sprites == null)
        {
            sprites = init_sprites;
        }
    }

    private void OnEnable()
    {
        if (started)
            GenerateTodayWorkers();
    }
    // Start is called before the first frame update
    void Start()
    {
        GenerateTodayWorkers();
        started = true;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GenerateTodayWorkers()
    {
        // if already initialized
        if (PlayerPrefs.HasKey("todayDate") && PlayerPrefs.GetString("todayDate").Equals(DateTime.Now.Date.ToShortDateString()))
        {
            //Debug.Log("loading");
            LoadFromPlayerPrefs();
        }
        else
        {
            //Debug.Log("generating");
            // TODO: Generate random workers here, amount slots
            //System.Random rnd = new System.Random();
            WorkerEntry one = new WorkerEntry("lala ushi", 1000, 1, "gray", 0, new List<string> { "saw", "" }, new List<int> { 1,0 });
            WorkerEntry two = new WorkerEntry("momo boshi", 2000, 2, "green", 0, new List<string> { "saw", "laser" }, new List<int> { 2, 1 });
            todayWorkerEntries = new List<WorkerEntry> { one, two };
            SaveToPlayerPrefs();
        }
        for (int i=0; i<5; i++)
        {
            itemControllers[i].gameObject.SetActive(false);
        }
        for (int i=0; i<todayWorkerEntries.Count; i++)
        {
            itemControllers[i].gameObject.SetActive(true);
            itemControllers[i].SetWorkerItem(i, todayWorkerEntries[i].name, todayWorkerEntries[i].base_price, todayWorkerEntries[i].tier,
                todayWorkerEntries[i].color, todayWorkerEntries[i].sprite, todayWorkerEntries[i].workstations, todayWorkerEntries[i].workstationStats);
        }
    }

    public void SaveToPlayerPrefs()
    {
        PlayerPrefs.SetString("todayDate", DateTime.Now.Date.ToShortDateString());
        PlayerPrefs.SetInt("todayWorkerNums", todayWorkerEntries.Count);
        for (int i=0; i<todayWorkerEntries.Count; i++)
        {
            PlayerPrefs.SetString("todayWorkerName_" + i, todayWorkerEntries[i].name);
            PlayerPrefs.SetInt("todayWorkerBasePrice_" + i, todayWorkerEntries[i].base_price);
            PlayerPrefs.SetInt("todayWorkerTier_" + i, todayWorkerEntries[i].tier);
            PlayerPrefs.SetString("todayWorkerColorStr_" + i, todayWorkerEntries[i].colorStr);
            PlayerPrefs.SetInt("todayWorkerSpriteNum_" + i, todayWorkerEntries[i].spriteNum);
            PlayerPrefs.SetString("todayWorkerProficiency0_" + i, todayWorkerEntries[i].workstations[0]);
            PlayerPrefs.SetString("todayWorkerProficiency1_" + i, todayWorkerEntries[i].workstations[1]);
            PlayerPrefs.SetInt("todayWorkerProficiencyStat0_" + i, todayWorkerEntries[i].workstationStats[0]);
            PlayerPrefs.SetInt("todayWorkerProficiencyStat1_" + i, todayWorkerEntries[i].workstationStats[1]);
        }
    }

    public void LoadFromPlayerPrefs()
    {
        todayWorkerEntries = new List<WorkerEntry>();
        for (int i = 0; i < PlayerPrefs.GetInt("todayWorkerNums"); i++)
        {
            string name = PlayerPrefs.GetString("todayWorkerName_" + i);
            int base_price = PlayerPrefs.GetInt("todayWorkerBasePrice_" + i);
            int tier = PlayerPrefs.GetInt("todayWorkerTier_" + i);
            string colorStr = PlayerPrefs.GetString("todayWorkerColorStr_" + i);
            int spriteNum = PlayerPrefs.GetInt("todayWorkerSpriteNum_" + i);
            string proficiency0 = PlayerPrefs.GetString("todayWorkerProficiency0_" + i);
            string proficiency1 = PlayerPrefs.GetString("todayWorkerProficiency1_" + i);
            int proficiencStat0 = PlayerPrefs.GetInt("todayWorkerProficiencyStat0_" + i);
            int proficiencStat1 = PlayerPrefs.GetInt("todayWorkerProficiencyStat1_" + i);
            WorkerEntry entry = new WorkerEntry(name, base_price, tier, colorStr, spriteNum, 
                new List<string> { proficiency0, proficiency1 }, new List<int> { proficiencStat0, proficiencStat1 });
            todayWorkerEntries.Add(entry);
        }
    }

    public void RemoveWorkerFromShop(int index)
    {
        todayWorkerEntries.RemoveAt(index);
        SaveToPlayerPrefs();
        GenerateTodayWorkers();
    }
    
}
