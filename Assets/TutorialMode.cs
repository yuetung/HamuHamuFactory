using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialMode : MonoBehaviour
{
    public GameObject tutorialDialogue;
    public GameObject showMessagetutorialSkip;
    public Text tutorialDialogueText;
    public Button YesButton;
    public Button NoButton;
    public Button OkButton;
    public GameObject ProductionRequestMenuArrow;
    public GameObject WorldMapArrow;
    public GameObject workstationShopArrow;
    public GameObject employmentAgecyArrow;
    public GameObject materialShopArrow;
    public GameObject AlexPic;
    public GameObject BarrelArrow;
    public GameObject PlayButtonArrow;

    public Button ProductionTaskMenuButton;
    public Button WorldMapButton;
    public Button InventoryButton;
    public Button WorkerInventoryButton;
    public Button WorkstationShopButton;
    public Button MaterialShopButton;
    public Button EmploymentAgencyButton;
    public Button BeachButton;
    public Button ParkButton;
    public Button MarketplaceButton;

    public GameObject productionRequestMenu;
    public GameObject workstationShopMenu;
    public GameObject employmentAgencyMenu;
    public GameObject materialShopMenu;
    public GameObject worldMap;

    public GameObject level2Dialogue;
    public Text level2DialogueText;
    public Button level2YesButton;
    public Button level2NoButton;
    public Button level2OkButton;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("tuorialStage"))
        {
            PlayerPrefs.SetInt("tuorialStage", 0);
            tutorialDialogue.SetActive(true);
            DisableAllButtons();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void YesSelected()
    {
        switch (PlayerPrefs.GetInt("tuorialStage"))
        {
            case 0:
                tutorialDialogueText.text = "Great! Let's get started.";
                PlayerPrefs.SetInt("tuorialStage", 1);
                ShowOk();
                //GameManager.instance.GainProductionJob("barrel", "Barrel", true);
                //GameManager.instance.GainProductionJob("barrel_table", "Barrel Table", true);
                break;
        }
    }

    public void NoSelected()
    {

        switch(PlayerPrefs.GetInt("tuorialStage"))
        {
            case 0:
                EnableAllButtons();
                tutorialDialogueText.text = "Sure, I'll just leave these supplies here then.";
                ShowOk();
                //GameManager.instance.GainProductionJob("barrel", "Barrel", true);
                //GameManager.instance.GainProductionJob("barrel_table", "Barrel Table", true);
                break;
        }
    }

    public void OkSelected()
    {

        switch (PlayerPrefs.GetInt("tuorialStage"))
        {
            case 0:
                tutorialDialogue.SetActive(false);
                GameManager.instance.GainProductionJob("barrel", "Barrel", true);
                showMessagetutorialSkip.SetActive(true);
                GameManager.instance.GainMoney(2000);
                GameManager.instance.GainMaterial("wood", 10);
                GameManager.instance.GainWorker("Alex Blunder", "white", 0, new List<string> { "hammer", ""}, new List<int> { 1,0 });
                GameManager.instance.GainWorkstation("hammer", "Hammer Station");
                PlayerPrefs.SetInt("tuorialStage", 100);
                //GameManager.instance.GainProductionJob("barrel_table", "Barrel Table", true);
                break;
            case 1:
                tutorialDialogueText.text = "First, you'll need a PRODUCTION REQUEST from someone. You can start with producing some BARRELS for me.";
                PlayerPrefs.SetInt("tuorialStage", 2);
                break;
            case 2:
                tutorialDialogue.SetActive(false);
                GameManager.instance.GainProductionJob("barrel", "Barrel", true);
                PlayerPrefs.SetInt("tuorialStage", 3);
                break;
            case 4:
                ProductionTaskMenuButton.interactable = true;
                tutorialDialogue.SetActive(false);
                ProductionRequestMenuArrow.SetActive(true);
                PlayerPrefs.SetInt("tuorialStage", 5);
                break;
            case 6:
                productionRequestMenu.SetActive(false);
                tutorialDialogueText.text = "Let's start with purchasing a workstation. Go to the WORLD MAP.";
                PlayerPrefs.SetInt("tuorialStage", 7);
                break;
            case 7:
                WorldMapButton.interactable = true;
                tutorialDialogue.SetActive(false);
                WorldMapArrow.SetActive(true);
                PlayerPrefs.SetInt("tuorialStage", 8);
                break;
            case 9:
                WorkstationShopButton.interactable = true;
                workstationShopArrow.SetActive(true);
                tutorialDialogue.SetActive(false);
                PlayerPrefs.SetInt("tuorialStage", 10);
                break;
            case 11:
                tutorialDialogue.SetActive(false);
                GameManager.instance.GainMoney(1500);
                PlayerPrefs.SetInt("tuorialStage", 12);
                break;
            case 13:
                tutorialDialogueText.text = "You can hire worker by visiting the EMPLOYMENT AGENCY from the WORLD MAP.";
                PlayerPrefs.SetInt("tuorialStage", 14);
                ShowOk();
                break;
            case 14:
                WorldMapButton.interactable = true;
                tutorialDialogue.SetActive(false);
                WorldMapArrow.SetActive(true);
                PlayerPrefs.SetInt("tuorialStage", 15);
                break;
            case 18:
                AlexPic.SetActive(true);
                tutorialDialogueText.text = "Oh! Here's my former colleague Alex who got sacked recently. Why not just hire him for free?";
                PlayerPrefs.SetInt("tuorialStage", 19);
                ShowOk();
                break;
            case 19:
                tutorialDialogue.SetActive(false);
                AlexPic.SetActive(false);
                GameManager.instance.GainWorker("Alex Blunder", "white", 0, new List<string> { "hammer", "" }, new List<int> { 1, 0 },true);
                PlayerPrefs.SetInt("tuorialStage", 20);
                break;
            case 21:
                WorldMapButton.interactable = true;
                tutorialDialogue.SetActive(false);
                WorldMapArrow.SetActive(true);
                PlayerPrefs.SetInt("tuorialStage", 22);
                break;
            case 24:
                tutorialDialogue.SetActive(false);
                GameManager.instance.GainMoney(50);
                PlayerPrefs.SetInt("tuorialStage", 25);
                break;
            case 26:
                tutorialDialogueText.text = "To start producing BARRELS, re-visit the PRODUCTION REQUEST MENU and select the BARREL job";
                PlayerPrefs.SetInt("tuorialStage", 27);
                ShowOk();
                break;
            case 27:
                ProductionTaskMenuButton.interactable = true;
                tutorialDialogue.SetActive(false);
                ProductionRequestMenuArrow.SetActive(true);
                PlayerPrefs.SetInt("tuorialStage", 28);
                break;
            case 30:
                tutorialDialogue.SetActive(false);
                PlayerPrefs.SetInt("tuorialStage", 31);
                break;
            case 32:
                tutorialDialogue.SetActive(false);
                PlayButtonArrow.SetActive(true);
                PlayerPrefs.SetInt("tuorialStage", 33);
                break;
            case 34:
                tutorialDialogueText.text = "I'll be counting on you to restore HamuHamu Factory.";
                PlayerPrefs.SetInt("tuorialStage", 35);
                ShowOk();
                break;
            case 35:
                tutorialDialogueText.text = "It's time I need to leave. I'll leave these gold with you, use them well.";
                PlayerPrefs.SetInt("tuorialStage", 36);
                ShowOk();
                break;
            case 36:
                tutorialDialogue.SetActive(false);
                GameManager.instance.GainMoney(2000);
                EnableAllButtons();
                PlayerPrefs.SetInt("tuorialStage", 100);
                break;
        }
    }

    public void DialogueBoxOkSelected()
    {
        switch (PlayerPrefs.GetInt("tuorialStage"))
        {
            case 3:
                tutorialDialogue.SetActive(true);
                tutorialDialogueText.text = "Now visit the PRODUCTION REQUEST MENU to view the requirements for making BARRELS.";
                ShowOk();
                PlayerPrefs.SetInt("tuorialStage", 4);
                break;
            case 20:
                tutorialDialogue.SetActive(true);
                tutorialDialogueText.text = "Almost there! Now you'll need to purchase some MATERIAL for production.";
                worldMap.SetActive(false);
                employmentAgencyMenu.SetActive(false);
                ShowOk();
                PlayerPrefs.SetInt("tuorialStage", 21);
                break;
        }
    }

    public void ProuctionMenuSelected()
    {
        switch (PlayerPrefs.GetInt("tuorialStage"))
        {
            case 5:
                tutorialDialogue.SetActive(true);
                tutorialDialogueText.text = "You can't produce the BARRELS just yet, you need to get some WORKSTATION, WORKER and MATERIAL first.";
                ProductionRequestMenuArrow.SetActive(false);
                ShowOk();
                PlayerPrefs.SetInt("tuorialStage", 6);
                break;
            case 28:
                ProductionRequestMenuArrow.SetActive(false);
                BarrelArrow.SetActive(true);
                PlayerPrefs.SetInt("tuorialStage", 29);
                break;
        }
    }

    public void WorldMapSelected()
    {
        switch (PlayerPrefs.GetInt("tuorialStage"))
        {
            case 8:
                tutorialDialogue.SetActive(true);
                tutorialDialogueText.text = "Now go to the WORKSTATION SHOP";
                WorldMapArrow.SetActive(false);
                ShowOk();
                PlayerPrefs.SetInt("tuorialStage", 9);
                break;
            case 15:
                WorldMapArrow.SetActive(false);
                EmploymentAgencyButton.interactable = true;
                employmentAgecyArrow.SetActive(true);
                PlayerPrefs.SetInt("tuorialStage", 17);
                break;
            case 22:
                WorldMapArrow.SetActive(false);
                MaterialShopButton.interactable = true;
                materialShopArrow.SetActive(true);
                PlayerPrefs.SetInt("tuorialStage", 23);
                break;
        }
    }

    public void WorkstationShopSelected()
    {
        switch (PlayerPrefs.GetInt("tuorialStage"))
        {
            case 10:
                tutorialDialogue.SetActive(true);
                tutorialDialogueText.text = "You can purchase WORKSTATION from here, here's some gold to purchase one.";
                workstationShopArrow.SetActive(false);
                ShowOk();
                PlayerPrefs.SetInt("tuorialStage", 11);
                break;
        }
    }

    public void EmploymentAgencySelected()
    {
        switch (PlayerPrefs.GetInt("tuorialStage"))
        {
            case 17:
                tutorialDialogue.SetActive(true);
                tutorialDialogueText.text = "You can hire WORKER from here, let's see...... *BANG!!* Who's there?";
                employmentAgecyArrow.SetActive(false);
                ShowOk();
                PlayerPrefs.SetInt("tuorialStage", 18);
                break;
        }
    }

    public void MaterialShopSelected()
    {
        switch (PlayerPrefs.GetInt("tuorialStage"))
        {
            case 23:
                tutorialDialogue.SetActive(true);
                tutorialDialogueText.text = "Here's some money for you to purchase 10 pieces of wood.";
                materialShopArrow.SetActive(false);
                ShowOk();
                PlayerPrefs.SetInt("tuorialStage", 24);
                break;
        }
    }

    public void PurchasedWorkstation()
    {
        switch (PlayerPrefs.GetInt("tuorialStage"))
        {
            case 12:
                tutorialDialogue.SetActive(true);
                tutorialDialogueText.text = "Great, now you need to hire some WORKER to work for you.";
                workstationShopMenu.SetActive(false);
                worldMap.SetActive(false);
                ShowOk();
                PlayerPrefs.SetInt("tuorialStage", 13);
                break;
        }
    }

    public void PurchasedMaterial()
    {
        switch (PlayerPrefs.GetInt("tuorialStage"))
        {
            case 25:
                tutorialDialogue.SetActive(true);
                materialShopMenu.SetActive(false);
                tutorialDialogueText.text = "Great, let's start building some BARRELS!";
                workstationShopMenu.SetActive(false);
                worldMap.SetActive(false);
                ShowOk();
                PlayerPrefs.SetInt("tuorialStage", 26);
                break;
        }
    }

    public void BarrelJobSelected() // after confirmation
    {
        switch (PlayerPrefs.GetInt("tuorialStage"))
        {
            case 291:
                tutorialDialogue.SetActive(true);
                tutorialDialogueText.text = "You can assign WORKER to your station by clicking on the station";
                productionRequestMenu.SetActive(false);
                ShowOk();
                PlayerPrefs.SetInt("tuorialStage", 30);
                break;
        }
    }

    public void BarrelJobClicked()  // before confirmation
    {
        switch (PlayerPrefs.GetInt("tuorialStage"))
        {
            case 29:
                BarrelArrow.SetActive(false);
                PlayerPrefs.SetInt("tuorialStage", 291);
                break;
        }
    }

    public void WorkerAssigned()
    {
        switch (PlayerPrefs.GetInt("tuorialStage"))
        {
            case 31:
                tutorialDialogue.SetActive(true);
                tutorialDialogueText.text = "Press the PLAY button to start production";
                ShowOk();
                PlayerPrefs.SetInt("tuorialStage", 32);
                break;
        }
    }

    public void PlayArrowClicked()
    {
        switch (PlayerPrefs.GetInt("tuorialStage"))
        {
            case 33:
                PlayButtonArrow.SetActive(false);
                tutorialDialogue.SetActive(true);
                tutorialDialogueText.text = "Great job! Now that your first production line is running, I'm sure you'll be rich in no time!";
                ShowOk();
                PlayerPrefs.SetInt("tuorialStage", 34);
                break;
        }
    }

    private void ShowOk()
    {
        YesButton.gameObject.SetActive(false);
        NoButton.gameObject.SetActive(false);
        OkButton.gameObject.SetActive(true);
    }
    private void ShowYesNo()
    {
        YesButton.gameObject.SetActive(true);
        NoButton.gameObject.SetActive(true);
        OkButton.gameObject.SetActive(false);
    }
    private void DisableAllOptions()
    {
        YesButton.gameObject.SetActive(false);
        NoButton.gameObject.SetActive(false);
        OkButton.gameObject.SetActive(false);
    }
    private void DisableAllButtons()
    {
        ProductionTaskMenuButton.interactable=false;
        WorldMapButton.interactable = false;
        InventoryButton.interactable = false;
        WorkerInventoryButton.interactable = false;
        WorkstationShopButton.interactable = false;
        MaterialShopButton.interactable = false;
        EmploymentAgencyButton.interactable = false;
        BeachButton.interactable = false;
        ParkButton.interactable = false;
        MarketplaceButton.interactable = false;
    }
    private void EnableAllButtons()
    {
        ProductionTaskMenuButton.interactable = true;
        WorldMapButton.interactable = true;
        InventoryButton.interactable = true;
        WorkerInventoryButton.interactable = true;
        WorkstationShopButton.interactable = true;
        MaterialShopButton.interactable = true;
        EmploymentAgencyButton.interactable = true;
        BeachButton.interactable = true;
        ParkButton.interactable = true;
        MarketplaceButton.interactable = true;
    }

    public void OnApplicationQuit()
    {
        if (!PlayerPrefs.HasKey("tuorialStage") || PlayerPrefs.GetInt("tuorialStage")!=100)
            PlayerPrefs.DeleteAll();
    }



    public void ShowDialogueLevel2()
    {
        level2Dialogue.SetActive(true);
        level2DialogueText.text = "Hello dear, I heard rumors that the factory is up and running again. Are you the one running the factory ?";
        level2YesButton.gameObject.SetActive(true);
        level2NoButton.gameObject.SetActive(true);
        level2OkButton.gameObject.SetActive(false);
    }

    public void Level2YesButtonClicked()
    {
        level2DialogueText.text = "Oh dear~ That's great! You see, I'm running out of WOODEN TABLES to display my materials, would you produce some for me?";
        level2YesButton.gameObject.SetActive(false);
        level2NoButton.gameObject.SetActive(false);
        level2OkButton.gameObject.SetActive(true);
    }
    public void Level2NoButtonClicked()
    {
        level2DialogueText.text = "Oh... Then please inform your manager I would like him to produce some WOODEN TABLES for me. Thank you sweetheart~";
        level2YesButton.gameObject.SetActive(false);
        level2NoButton.gameObject.SetActive(false);
        level2OkButton.gameObject.SetActive(true);
    }
    public void Level2OkButtonClicked()
    {
        level2Dialogue.SetActive(false);
        GameManager.instance.GainProductionJob("wooden_table", "Wooden Table", true);
    }
}
