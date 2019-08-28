using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyMission : MonoBehaviour
{
    public Text[] missionTexts;
    public Text[] missionAmounts;
    public Text[] missionExps;
    public Text totalCompleted;
    public Text bossVisitDate;
    private DateTime bossNextVisit;
    public Mission[] allMissions;
    private List<Mission> availableMissions;
    public GameObject BossVisitDialogue;
    public Text BossVisitDialogueText;
    public Button YesButton;
    public Button NoButton;
    public Button OkButton;
    private int bossVisitState = 0;
    private int bossVisitBaseReward = 0;

    [System.Serializable]
    public class Mission
    {
        public string name;
        public int levelRequirement;
        public int[] totalChoices;
        public int[] expRewardChoices;
        private int total;
        private int expReward;
        public void Initialize()
        {
            int randomIndex = UnityEngine.Random.Range(0, totalChoices.Length);
            total = totalChoices[randomIndex];
            expReward = expRewardChoices[randomIndex];
        }
        public int GetTotal()
        {
            return total;
        }
        public int GetExpReward()
        {
            return expReward;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateAvailableMissions();
        // Boss visit
        if (PlayerPrefs.HasKey("boss_next_visit"))
        {
            long temp = Convert.ToInt64(PlayerPrefs.GetString("boss_next_visit"));
            bossNextVisit = DateTime.FromBinary(temp);
            if (DateTime.Now.Date>= bossNextVisit)
            {
                bossVisitState = 0;
                BossVisit();
                bossNextVisit = DateTime.Now.Date.AddDays(3);
                PlayerPrefs.SetString("boss_next_visit", bossNextVisit.ToBinary().ToString());
            }
        }
        else
        {
            bossNextVisit = DateTime.Now.Date.AddDays(3);
            PlayerPrefs.SetString("boss_next_visit", bossNextVisit.ToBinary().ToString());
        }
        // Generate missions (first time)
        if (!PlayerPrefs.HasKey("mission_total_completed"))
        {
            PlayerPrefs.SetInt("mission_total_completed", 0);
        }
        for (int i = 0; i<3; i++)
        {
            if (!PlayerPrefs.HasKey("mission_" + i))
            {
                GenerateMission(i);
            }
        }
        //PlayerPrefs.SetString("mission_0", "barrel");
        //PlayerPrefs.SetString("mission_1", "items of 2 stars and above");
        //PlayerPrefs.SetString("mission_2", "items (any)");
        //PlayerPrefs.SetInt("mission_total_0", 20);
        //PlayerPrefs.SetInt("mission_total_1", 30);
        //PlayerPrefs.SetInt("mission_total_2", 10);
        //PlayerPrefs.SetInt("mission_current_0", 0);
        //PlayerPrefs.SetInt("mission_current_1", 0);
        //PlayerPrefs.SetInt("mission_current_2", 0);
        //PlayerPrefs.SetInt("mission_exp_0", 5);
        //PlayerPrefs.SetInt("mission_exp_1", 3);
        //PlayerPrefs.SetInt("mission_exp_2", 8);
        UpdateBoard();

    }

    public void BossVisit()
    {
        BossVisitDialogue.SetActive(true);
        BossVisitDialogueText.text = "Meow~ So how hass it been? Have you restored my factory back to GREATNESS yet?";
        YesButton.gameObject.SetActive(true);
        NoButton.gameObject.SetActive(true);
        OkButton.gameObject.SetActive(false);
    }

    public void YesButtonClicked()
    {
        BossVisitDialogueText.text = "Awesome! Let's review your progress: you've completed a total of "+PlayerPrefs.GetInt("mission_total_completed").ToString() +" jobs since we last meet!";
        YesButton.gameObject.SetActive(false);
        NoButton.gameObject.SetActive(false);
        OkButton.gameObject.SetActive(true);
    }

    public void NoButtonClicked()
    {
        BossVisitDialogueText.text = "Hmm... Let's review your progress: you've completed a total of " + PlayerPrefs.GetInt("mission_total_completed").ToString() + " jobs since we last meet!";
        YesButton.gameObject.SetActive(false);
        NoButton.gameObject.SetActive(false);
        OkButton.gameObject.SetActive(true);
    }

    public void OkButtonClicked()
    {
        print("OkButtonClicked");
        switch (bossVisitState)
        {
            case 0:
                print("case0");
                if (PlayerPrefs.GetInt("mission_total_completed") < 3)
                {
                    BossVisitDialogueText.text = "What have you been doing!!! Really?!! I'm so disappointed!!!";
                    bossVisitBaseReward = 0;
                }
                else if (PlayerPrefs.GetInt("mission_total_completed") < 8)
                {
                    BossVisitDialogueText.text = "That's a bit.... disappointing.... (maybe I should eat you up instead?) Anyway, here's something for your effort";
                    bossVisitBaseReward = 100;
                }
                else if (PlayerPrefs.GetInt("mission_total_completed") < 15)
                {
                    BossVisitDialogueText.text = "Not bad! Let me reward you with a little something~";
                    bossVisitBaseReward = 300;
                }
                else if (PlayerPrefs.GetInt("mission_total_completed") < 30)
                {
                    BossVisitDialogueText.text = "Good work! You're really making progress right there! Let me reward you with a something great~";
                    bossVisitBaseReward = 500;
                }
                else if (PlayerPrefs.GetInt("mission_total_completed") < 50)
                {
                    BossVisitDialogueText.text = "Hohoho! You are doing really well! (fortunately I didn't eat him up earlier). Here's something for your effort";
                    bossVisitBaseReward = 800;
                }
                else
                {
                    BossVisitDialogueText.text = "My my my... With this I'm sure you'll be able to restore HamuHamu Factory in no time!!! Here's something for your effort";
                    bossVisitBaseReward = 1000;
                }
                bossVisitState = 1;
                break;
            case 1:
                BossVisitDialogue.SetActive(false);
                GameManager.instance.GainMoney(bossVisitBaseReward*GameManager.instance.currentLevel);
                PlayerPrefs.SetInt("mission_total_completed", 0);
                UpdateBoard();
                break;

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Produce(string produce_name)
    {
        // stars item
        if (produce_name.Contains("stars_item"))
        {
            for (int i = 0; i < 3; i++)
            {
                if (PlayerPrefs.HasKey("mission_" + i) && PlayerPrefs.GetString("mission_" + i).Contains("stars and above"))
                {
                    int required_stars = int.Parse(PlayerPrefs.GetString("mission_" + i)[9].ToString());
                    int produce_stars = int.Parse(produce_name[0].ToString()) /2;
                    print(required_stars + "  " + produce_stars);
                    if (produce_stars >= required_stars)
                    {
                        int current_amount = PlayerPrefs.GetInt("mission_current_" + i);
                        PlayerPrefs.SetInt("mission_current_" + i, current_amount + 1);
                        UpdateBoard();
                    }
                }
            }
        }
        // other
        else
        {
            for (int i = 0; i < 3; i++)
            {
                if (PlayerPrefs.HasKey("mission_" + i) && (PlayerPrefs.GetString("mission_" + i) == produce_name|| PlayerPrefs.GetString("mission_" + i) == "items (any)"))
                {
                    int current_amount = PlayerPrefs.GetInt("mission_current_" + i);
                    PlayerPrefs.SetInt("mission_current_" + i, current_amount + 1);
                    UpdateBoard();
                }
            }
        }
    }

    public void UpdateBoard()
    {
        for (int i = 0; i < 3; i++)
        {
            if (PlayerPrefs.HasKey("mission_" + i))
            {
                if (PlayerPrefs.GetInt("mission_current_" + i) >= PlayerPrefs.GetInt("mission_total_" + i))
                {
                    CompleteMission(i);
                }
                missionTexts[i].text = "produce " + PlayerPrefs.GetInt("mission_total_" + i).ToString() + " " + PlayerPrefs.GetString("mission_" + i);
                missionAmounts[i].text = PlayerPrefs.GetInt("mission_current_" + i).ToString() + "/" + PlayerPrefs.GetInt("mission_total_" + i).ToString();
                missionExps[i].text = PlayerPrefs.GetInt("mission_exp_" + i).ToString();
                bossVisitDate.text = bossNextVisit.ToShortDateString();
                totalCompleted.text = PlayerPrefs.GetInt("mission_total_completed").ToString();
            }
        }
    }

    public void CompleteMission(int i)
    {
        GameManager.instance.GainExp(PlayerPrefs.GetInt("mission_exp_" + i));
        int totalCompleted = PlayerPrefs.GetInt("mission_total_completed");
        PlayerPrefs.SetInt("mission_total_completed", totalCompleted+1);
        GenerateMission(i);
    }

    public void GenerateMission(int i)
    {
        int missionIndex = UnityEngine.Random.Range(0, availableMissions.Count);
        Mission m = availableMissions[missionIndex];
        m.Initialize();
        PlayerPrefs.SetString("mission_" + i, m.name);
        PlayerPrefs.SetInt("mission_total_"+i, m.GetTotal());
        PlayerPrefs.SetInt("mission_current_"+i, 0);
        PlayerPrefs.SetInt("mission_exp_"+i, m.GetExpReward());
    }

    public void UpdateAvailableMissions()
    {
        availableMissions = new List<Mission>();
        foreach (Mission m in allMissions)
        {
            if (m.levelRequirement <= GameManager.instance.currentLevel)
            {
                availableMissions.Add(m);
            }
        }
    }
}
