using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class WorkstationConstructionMasterController : MonoBehaviour
{
    public WorkstationShopItemController[] itemControllers;

    [System.Serializable]
    public class ShopItemEntry
    {
        public string display_name;
        public string variable_name;
        public int level_requirement;
        public int base_price;
        public Sprite sprite;
    }
    public ShopItemEntry[] fixedShopItemEntries;
    //public ShopItemEntry[] variableShopItemEntries;
    //private int slots=2; // max = 5
    private bool started = false;
    public Dictionary<string, ShopItemEntry> var_name_to_item_entry;

    private void Awake()
    {
        var_name_to_item_entry = new Dictionary<string, ShopItemEntry>();
        for (int i = 0; i < fixedShopItemEntries.Length; i++)
        {
            print(fixedShopItemEntries[i].variable_name);
            var_name_to_item_entry[fixedShopItemEntries[i].variable_name] = fixedShopItemEntries[i];
        }
    }

    private void OnEnable()
    {
        if (started)
            ResetAllWorkstationItemControllerAfterPurchase();
    }
    // Start is called before the first frame update
    void Start()
    {
        PickTodayWorkstationShopItems();
        started = true;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // The last 3 workstation shop items varies daily
    public void PickTodayWorkstationShopItems()
    {
        // hide all itemControllers not in-use
        for(int i= fixedShopItemEntries.Length; i<itemControllers.Length; i++)
        {
            itemControllers[i].gameObject.SetActive(false);
        }
        // first few item are fixed
        for(int i=0; i<fixedShopItemEntries.Length; i++)
        {
            itemControllers[i].SetWorkstationShopItem(fixedShopItemEntries[i].display_name, fixedShopItemEntries[i].variable_name,
                fixedShopItemEntries[i].level_requirement, fixedShopItemEntries[i].base_price, fixedShopItemEntries[i].sprite);
        }
        // for rest of shopitems, picks #slots items from shopItemEntries, randomized; 
        //if (PlayerPrefs.HasKey("todayWorkstationName_0") && PlayerPrefs.HasKey("todayWorkstationVarDate") &&
        //    PlayerPrefs.GetString("todayWorkstationVarDate").Equals(DateTime.Now.Date.ToShortDateString()))
        //{
        //    // If today's variable shop items has already been initialized
        //    {
        //        for(int i=0; i < Mathf.Min(slots - fixedShopItemEntries.Length, variableShopItemEntries.Length); i++)
        //        {
        //            //Debug.Log("oops");
        //            int index = PlayerPrefs.GetInt("todayWorkstationName_" + i);
        //            ShopItemEntry s = variableShopItemEntries[index];
        //            itemControllers[i + fixedShopItemEntries.Length].SetWorkstationShopItem(s.display_name, s.variable_name,
        //        s.level_requirement, s.base_price, s.sprite);
        //        }
        //    }
        //}
        //else
        //{
        //    // If today's variable shop items has not already been initialized
        //    System.Random rnd = new System.Random();
        //    int j = 0;
        //    foreach (ShopItemEntry s in variableShopItemEntries.OrderBy(x => rnd.Next())
        //        .Take(Mathf.Min(slots - fixedShopItemEntries.Length, variableShopItemEntries.Length)))
        //    {
        //        //Debug.Log("oops");
        //        itemControllers[j + fixedShopItemEntries.Length].SetWorkstationShopItem(s.display_name, s.variable_name,
        //        s.level_requirement, s.base_price, s.sprite);
        //        // store today's selection in playerprefs
        //        PlayerPrefs.SetInt("todayWorkstationName_" + j, Array.IndexOf(variableShopItemEntries, s));
        //        j++;
        //    }
        //    PlayerPrefs.SetString("todayWorkstationVarDate", DateTime.Now.Date.ToShortDateString());
        //}
    }

    public void ResetAllWorkstationItemControllerAfterPurchase()
    {
        for (int i=0; i<itemControllers.Length; i++)
        {
            itemControllers[i].ResetColour();
        }
    }
}
