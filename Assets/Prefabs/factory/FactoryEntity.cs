using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FactoryEntity : MonoBehaviour
{
    [SerializeField]
    private List<Animator> childAnimators = null;
    public WorkstationBuilder workstationBuilder;
    //test variables: please delete and use function in future
    public bool triggerAction = false;
    public bool workerAssigned = false;
    public GameObject worker;
    public Image workerImage;
    public string colorStr = "white";
    public string workerName = "";
    public int stars = 0;
    public string workstation_var_name;
    bool prev = false;
    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.transform.Find("worker"))
        {
            worker = gameObject.transform.Find("worker").gameObject;
            workerImage = worker.GetComponent<Image>();
            if (!workerAssigned)
            {
                UpdateWorkerImageColorAlpha(Color.white, 0.0f);
            }
        }
        workstationBuilder = transform.parent.gameObject.GetComponent<WorkstationBuilder>();
        //StartProduction();
    }

    // Update is called once per frame
    void Update()
    {
        if (triggerAction != prev)
        {
            TriggerAction();
            prev = triggerAction;
        }
    }
    
    public void IsStarted(bool x)
    {
        if (childAnimators != null)
            foreach (Animator anim in childAnimators)
            {
                if (HasParameter("isStarted", anim))
                {
                    anim.SetBool("isStarted", x);
                }
                    
            }
    }

    public void IsOperating(bool x)
    {
        if (childAnimators != null)
            foreach (Animator anim in childAnimators)
            {
                if (HasParameter("isOperating", anim))
                {
                    anim.SetBool("isOperating", x);
                }
                    
            }
    }

    public void TriggerAction()
    {
        if (childAnimators != null)
            foreach (Animator anim in childAnimators)
            {
                if (HasParameter("isStarted", anim))
                    anim.SetTrigger("triggerAction");
            }
    }

    public void StartProduction()
    {
        IsStarted(true);
        IsOperating(true);
    }

    public void StopProduction()
    {
        IsStarted(false);
        IsOperating(false);
    }

    public void PointerEnter()
    {
        if (!workerAssigned)
        {
            UpdateWorkerImageColorAlpha(Color.white, 0.2f);
        }
        else if (!workstationBuilder.startedProduction)
        {
            UpdateWorkerImageColorAlpha(ToColor(colorStr), 0.8f);
        }
    }

    public void PointerExit()
    {
        if (!workerAssigned)
        {
            UpdateWorkerImageColorAlpha(Color.white, 0.0f);
        }
        else if (!workstationBuilder.startedProduction)
        {
            UpdateWorkerImageColorAlpha(ToColor(colorStr), 1.0f);
        }
    }
    public void Click()
    {
        //TODO: show empty worker placeholder
        //Debug.Log("clicked");
        if (!workstationBuilder.startedProduction)
        {
            //AssignWorker(1, "red");
            //print(WorkerInventoryMasterController._instance);
            WorkerInventoryMasterController._instance.AssignWorkerInventory(this);
        }
        //Debug.Log("assigning worker");
    }
    public void AssignWorker(int stars, string colorStr, string workerName)
    {
        this.stars = stars;
        this.colorStr = colorStr;
        this.workerName = workerName;
        UpdateWorkerImageColorAlpha(ToColor(colorStr), 1.0f);
        workerAssigned = true;
        workstationBuilder.SaveEntitiesToPlayerPrefs();
    }
    public void AssignWorkerLoadTime(int stars, string colorStr)
    {
        if (gameObject.transform.Find("worker"))
        {
            worker = gameObject.transform.Find("worker").gameObject;
            workerImage = worker.GetComponent<Image>();
        }
        workstationBuilder = transform.parent.gameObject.GetComponent<WorkstationBuilder>();
        this.stars = stars;
        this.colorStr = colorStr;
        //print(colorStr);
        UpdateWorkerImageColorAlpha(ToColor(colorStr), 1.0f);
        workerAssigned = true;
    }

    //Utilities
    public static bool HasParameter(string paramName, Animator animator)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName)
                return true;
        }
        return false;
    }
    public void UpdateWorkerImageColorAlpha(Color color, float alpha)
    {
        Color workerImageColor = color;
        workerImageColor.a = alpha;
        workerImage.color = workerImageColor;
    }

    //Utilities
    public Color ToColor(string color)
    {
        return (Color)typeof(Color).GetProperty(color.ToLowerInvariant()).GetValue(null, null);
    }
}
