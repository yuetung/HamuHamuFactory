using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MaterialShopMasterController : MonoBehaviour
{
    public MaterialShopItemController[] itemControllers;

    [System.Serializable]
    public class ShopItemEntry
    {
        public string display_name;
        public string variable_name;
        public int unit_price;
        public Sprite sprite;
    }
    public ShopItemEntry[] fixedShopItemEntries;
    //public ShopItemEntry[] variableShopItemEntries;
    //private int slots = 2; // max = 5
    //private bool started = false;
    public Dictionary<string, ShopItemEntry> var_name_to_item_entry;

    private void Awake()
    {
        var_name_to_item_entry = new Dictionary<string, ShopItemEntry>();
        for (int i = 0; i < fixedShopItemEntries.Length; i++)
        {
            var_name_to_item_entry[fixedShopItemEntries[i].variable_name] = fixedShopItemEntries[i];
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        PickTodayMaterialShopItems();
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // The last 3 workstation shop items varies daily
    public void PickTodayMaterialShopItems()
    {
        // hide all itemControllers not in-use
        for (int i = fixedShopItemEntries.Length; i < itemControllers.Length; i++)
        {
            itemControllers[i].gameObject.SetActive(false);
        }
        // first few item are fixed
        for (int i = 0; i < fixedShopItemEntries.Length; i++)
        {
            itemControllers[i].SetMaterialShopItem(fixedShopItemEntries[i].display_name, fixedShopItemEntries[i].variable_name,
                fixedShopItemEntries[i].unit_price, fixedShopItemEntries[i].sprite);
        }
        // for rest of shopitems, picks #slots items from shopItemEntries, randomized; 
        //if (PlayerPrefs.HasKey("todayMaterialName_0") && PlayerPrefs.HasKey("todayMaterialVarDate") &&
        //    PlayerPrefs.GetString("todayMaterialVarDate").Equals(DateTime.Now.Date.ToShortDateString()))
        //{
        //    // If today's variable shop items has already been initialized
        //    {
        //        for (int i = 0; i < Mathf.Min(slots - fixedShopItemEntries.Length, variableShopItemEntries.Length); i++)
        //        {
        //            int index = PlayerPrefs.GetInt("todayMaterialName_" + i);
        //            ShopItemEntry s = variableShopItemEntries[index];
        //            itemControllers[i + fixedShopItemEntries.Length].SetMaterialShopItem(s.display_name, s.variable_name,
        //                s.unit_price, s.sprite);
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
        //        itemControllers[j + fixedShopItemEntries.Length].SetMaterialShopItem(s.display_name, s.variable_name,
        //            s.unit_price, s.sprite);
        //        // store today's selection in playerprefs
        //        PlayerPrefs.SetInt("todayMaterialName_" + j, Array.IndexOf(variableShopItemEntries, s));
        //        j++;
        //    }
        //    PlayerPrefs.SetString("todayMaterialVarDate", DateTime.Now.Date.ToShortDateString());
        //}
    }

    public Sprite GetMaterialSprite(string var_name)
    {
        ShopItemEntry mat = var_name_to_item_entry[var_name];
        return mat.sprite;
    }


}
