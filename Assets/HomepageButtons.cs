using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomepageButtons : MonoBehaviour
{
    public GameObject continueButton;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("tuorialStage"))
        {
            continueButton.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene("Factory", LoadSceneMode.Single);
    }

    public void NewGame()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Factory", LoadSceneMode.Single);
    }
}
