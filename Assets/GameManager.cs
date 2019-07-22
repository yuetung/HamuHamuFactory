using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int moneyOnHand;
    public int starting_moneyOnHand = 3000;
    public int starting_exp = 0;
    public Text moneyOnHandText;
    public Text moneyOnHandChangesText;
    [Tooltip("Uses 100 as base, scales with amount of change")]
    public float moneyChangeDuration = 3.0f;
    public List<int> expRequired;
    public int currentExp = 0;
    public int currentLevel = 0;
    public Text levelText;
    public Text ExpText;
    public BarAnimator expBarController;
    public RoomSizeManager roomSizeManager;
    private Coroutine disablemoneyOnHandChangesTextCoroutine = null;
    private Coroutine changeMoneyToCoroutine = null;
    public bool resetPlayerPrefs = false;
    public GameObject messageBoard;
    public Text messageBoardText;

    public class Employee
    {
        string field;
        int sprite;
        string name;
        List<int> stationStats;
        int costPerDay;
        public Employee(string field, int sprite, string name, List<int> stationStats, int costPerDay)
        {
            this.field = field;
            this.name = name;
            this.stationStats = stationStats;
            this.costPerDay = costPerDay;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        if (resetPlayerPrefs)
        {
            PlayerPrefs.DeleteAll();
        }
        if (PlayerPrefs.HasKey("moneyOnHand"))
        {
            moneyOnHand = PlayerPrefs.GetInt("moneyOnHand");
            currentExp = PlayerPrefs.GetInt("currentExp");
            currentLevel = PlayerPrefs.GetInt("currentLevel");
        }
        else
        {
            moneyOnHand = starting_moneyOnHand;
            PlayerPrefs.SetInt("moneyOnHand", moneyOnHand);
            currentLevel = ExpToLevel(starting_exp);
            if (currentLevel <= 1)
            {
                currentExp = starting_exp;
            }
            else
            {
                currentExp = starting_exp - expRequired[currentLevel - 2];
            }
            
            PlayerPrefs.SetInt("currentExp", currentExp);
            PlayerPrefs.SetInt("currentLevel", currentLevel);
        }
        UpdateMoneyText();
        UpdateExpText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Employee> UpdateHiring(string field, int num, int level, List<int> stations)
    {
        List<Employee> employees = new List<Employee>();
        // TODO: do random generating
        return employees;
    }

    public void LossMoney(int amount)
    {
        // play some fancy money lowering animations here e.g. red number showing decrease
        moneyOnHandChangesText.gameObject.SetActive(true);
        moneyOnHandChangesText.text = "-" + amount.ToString();
        moneyOnHandChangesText.color = Color.red;
        if (disablemoneyOnHandChangesTextCoroutine!=null)
        {
            StopCoroutine(disablemoneyOnHandChangesTextCoroutine);
        }
        float duration = moneyChangeDuration * Mathf.Sqrt(amount) / 10f;
        disablemoneyOnHandChangesTextCoroutine = StartCoroutine(DisablemoneyOnHandChangesText(duration));
        if (changeMoneyToCoroutine != null)
        {
            StopCoroutine(changeMoneyToCoroutine);
        }
        changeMoneyToCoroutine =StartCoroutine(ChangeMoneyTo(moneyOnHand - amount, duration));
    }

    public void GainMoney(int amount)
    {
        // play some fancy money increasing animations here e.g. green number showing increase
        moneyOnHandChangesText.gameObject.SetActive(true);
        moneyOnHandChangesText.text = "+"+amount.ToString();
        moneyOnHandChangesText.color = Color.green;
        if (disablemoneyOnHandChangesTextCoroutine != null)
        {
            StopCoroutine(disablemoneyOnHandChangesTextCoroutine);
        }
        float duration = moneyChangeDuration * Mathf.Sqrt(amount) / 10f;
        disablemoneyOnHandChangesTextCoroutine = StartCoroutine(DisablemoneyOnHandChangesText(duration));
        if (changeMoneyToCoroutine != null)
        {
            StopCoroutine(changeMoneyToCoroutine);
        }
        changeMoneyToCoroutine =StartCoroutine(ChangeMoneyTo(moneyOnHand + amount, duration));
    }

    public void GainExp(int amount)
    {
        // Gain level if exp > exp required to level up
        
        if (currentExp + amount >= expRequired[currentLevel - 1])
        {
            int newExp = currentExp + amount;
            while (newExp >= expRequired[currentLevel - 1])
            {
                newExp -= expRequired[currentLevel - 1];
                currentLevel += 1;
                LevelUp();
            }
            currentExp = newExp;
        }
        else
        {
            currentExp = currentExp + amount;
        }
        UpdateExpText();
    }

    private void UpdateMoneyText()
    {
        moneyOnHandText.text = moneyOnHand.ToString();
    }

    private void UpdateExpText()
    {
        levelText.text = "Level "+currentLevel.ToString();
        ExpText.text = currentExp.ToString() + "/" + expRequired[currentLevel - 1];
        expBarController.SetEXP(1f*currentExp/ expRequired[currentLevel - 1]);
        PlayerPrefs.SetInt("currentExp", currentExp);
        PlayerPrefs.SetInt("currentLevel", currentLevel);
    }

    private IEnumerator DisablemoneyOnHandChangesText(float duration)
    {
        yield return new WaitForSeconds(duration);
        moneyOnHandChangesText.gameObject.SetActive(false);
    }

    IEnumerator ChangeMoneyTo(int target, float duration)
    {
        int start = moneyOnHand;
        moneyOnHand = target;
        PlayerPrefs.SetInt("moneyOnHand", moneyOnHand);
        for (float timer = 0; timer < duration; timer += Time.deltaTime)
        {
            float progress = timer / duration;
            float m = (int)Mathf.Lerp(start, target, progress);
            moneyOnHandText.text = m.ToString();
            yield return null;
        }
        UpdateMoneyText();
    }

    public int ExpToLevel(int exp)
    {
        int l = 1;
        for (int i=0; i<expRequired.Count; i++)
        {
            if (exp >= expRequired[i])
            {
                l++;
            }
        }
        return l;
    }

    public void LevelUp()
    {
        // play level up animation here
        if (currentLevel%1==0)
            roomSizeManager.AddRoom();  // add room every 1 levels?
        Debug.Log("level up!");
    }

    public void GainWorkstation(string name, string displayed_name, int amount=1, bool show_message = false)
    {
        if (PlayerPrefs.HasKey(name))
        {
            int inventory = PlayerPrefs.GetInt(name);
            PlayerPrefs.SetInt(name, inventory + amount);
        }
        else
        {
            PlayerPrefs.SetInt(name, amount);
        }
        Debug.Log("Gain " + amount.ToString() + " " + name + " workstation (Total=" + PlayerPrefs.GetInt(name)+")");
        if (show_message)
        {
            int inventory = PlayerPrefs.GetInt(name);
            string message = "You gained " + amount + " " + displayed_name + "!\n total: " + inventory;
            // show a message here
        }
    }

    public void GainMaterial(string name, int amount, string displayed_name = "", bool show_message = false)
    {
        if (PlayerPrefs.HasKey(name))
        {
            int inventory = PlayerPrefs.GetInt(name);
            PlayerPrefs.SetInt(name, inventory + amount);
        }
        else
        {
            PlayerPrefs.SetInt(name, amount);
        }
        Debug.Log("Gain " + amount.ToString() + " " + name + " material (Total=" + PlayerPrefs.GetInt(name) + ")");
        if (show_message)
        {
            int inventory = PlayerPrefs.GetInt(name);
            string message = "You gained " + amount + " " + displayed_name + "!\n total: " + inventory;
            messageBoard.SetActive(true);
            messageBoardText.text = message;
        }
    }

    public void LossMaterial(string name, int amount, string displayed_name = "", bool show_message = false)
    {
        if (!PlayerPrefs.HasKey(name))
        {
            Debug.LogWarning("Error!!! Trying to remove material "+name+" but it does not exist");
            return;
        }
        int new_inventory = PlayerPrefs.GetInt(name)-amount;
        if (new_inventory <= 0)
        {
            PlayerPrefs.DeleteKey(name);
        }
        else
        {
            PlayerPrefs.SetInt(name, new_inventory);
        }
    }

    public void GainProductionJob(string name, string displayed_name, bool show_message = false)
    {
        if (PlayerPrefs.HasKey(name))
        {
            Debug.Log("Production job already available");
            if (show_message)
            {
                string message = "Production job " + displayed_name + " already available";
                messageBoard.SetActive(true);
                messageBoardText.text = message;
            }
        }
        else
        {
            PlayerPrefs.SetInt(name,1);
            Debug.Log("Obtained a new production job: " + displayed_name + " !!!");
            if (show_message)
            {
                string message = "Obtained a production job: " + displayed_name + " !!!";
                messageBoard.SetActive(true);
                messageBoardText.text = message;
            }
        }
    }
}
