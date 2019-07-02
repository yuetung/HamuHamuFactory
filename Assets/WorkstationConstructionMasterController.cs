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
        public string name;
        public int base_price;
        public int numWorkers;
        public Sprite sprite;
    }
    public ShopItemEntry[] fixedShopItemEntries;
    public ShopItemEntry[] variableShopItemEntries;
    private int slots=5; // max = 5

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("ConstructionSlots"))
        {
            slots = PlayerPrefs.GetInt("ConstructionSlots");
        }
        else //new game
        {
            PlayerPrefs.SetInt("ConstructionSlots", slots);
        }
        PickTodayWorkstationShopItems();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // The last 3 workstation shop items varies daily
    public void PickTodayWorkstationShopItems()
    {
        // hide all itemControllers not in-use
        for(int i=slots; i<itemControllers.Length; i++)
        {
            itemControllers[i].gameObject.SetActive(false);
        }
        // first 2 item are fixed
        for(int i=0; i<2; i++)
        {
            itemControllers[i].SetWorkstationShopItem(fixedShopItemEntries[i].name, fixedShopItemEntries[i].base_price,
                fixedShopItemEntries[i].numWorkers, fixedShopItemEntries[i].sprite);
        }
        // for rest of shopitems, picks #slots items from shopItemEntries, randomized; 
        if (PlayerPrefs.HasKey("todayWorkstationName_0") && PlayerPrefs.HasKey("todayWorkstationVarDate") &&
            PlayerPrefs.GetString("todayWorkstationVarDate").Equals(DateTime.Now.Date.ToShortDateString()))
        {
            // If today's variable shop items has already been initialized
            {
                for(int i=0; i < slots - 2; i++)
                {
                    int index = PlayerPrefs.GetInt("todayWorkstationName_" + i);
                    ShopItemEntry s = variableShopItemEntries[index];
                    itemControllers[i + 2].SetWorkstationShopItem(s.name, s.base_price, s.numWorkers, s.sprite);
                }
            }
        }
        else
        {
            // If today's variable shop items has not already been initialized
            System.Random rnd = new System.Random();
            int j = 0;
            foreach (ShopItemEntry s in variableShopItemEntries.OrderBy(x => rnd.Next()).Take(slots - 2))
            {
                itemControllers[j + 2].SetWorkstationShopItem(s.name, s.base_price, s.numWorkers, s.sprite);
                // store today's selection in playerprefs
                PlayerPrefs.SetInt("todayWorkstationName_" + j, Array.IndexOf(variableShopItemEntries, s));
                j++;
            }
            PlayerPrefs.SetString("todayWorkstationVarDate", DateTime.Now.Date.ToShortDateString());
        }
    }

    public void ResetAllWorkstationItemControllerAfterPurchase()
    {
        for (int i=0; i<itemControllers.Length; i++)
        {
            itemControllers[i].ResetColour();
        }
    }
}
