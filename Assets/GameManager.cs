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
    public bool resetPlayerPrefs = false;
    private Coroutine disablemoneyOnHandChangesTextCoroutine = null;
    private Coroutine changeMoneyToCoroutine = null;

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
                //TODO: play level up animation here
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
}
