using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMasterController : MonoBehaviour
{
    public WorkstationConstructionMasterController workstationShopMaster;
    public MaterialShopMasterController materialShopMaster;
    public List<InventoryItemController> inv_workstations;
    public List<InventoryItemController> inv_materials;

    public void OnEnable()
    {
        DisplayInventory();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayInventory()
    {
        // set all workstations available to show
        int numWorkstations = 0;
        int numMaterials = 0;
        WorkstationConstructionMasterController.ShopItemEntry[] workstations = workstationShopMaster.fixedShopItemEntries;
        for (int i=0; i< workstations.Length; i++)
        {
            if (PlayerPrefs.HasKey(workstations[i].variable_name))
            {
                inv_workstations[i].gameObject.SetActive(true);
                numWorkstations++;
                inv_workstations[i].SetShopItem(workstations[i].display_name, workstations[i].variable_name, workstations[i].sprite);
            }
        }
        MaterialShopMasterController.ShopItemEntry[] materials = materialShopMaster.fixedShopItemEntries;
        for (int i = 0; i < materials.Length; i++)
        {
            if (PlayerPrefs.HasKey(materials[i].variable_name))
            {
                inv_materials[i].gameObject.SetActive(true);
                numMaterials++;
                inv_materials[i].SetShopItem(materials[i].display_name, materials[i].variable_name, materials[i].sprite);
            }
        }
        // Disables undisplayed workstations/ materials
        for (int i = numWorkstations; i < inv_workstations.Count; i++)
        {
            inv_workstations[i].gameObject.SetActive(false);
        }
        for (int i = numMaterials; i < inv_materials.Count; i++)
        {
            inv_materials[i].gameObject.SetActive(false);
        }

    }
}
