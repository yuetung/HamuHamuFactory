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
                print("boss says hello");
                PlayerPrefs.SetInt("mission_total_completed", 0);
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
