using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldMapController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void goToConstructionShop()
    {
        SceneManager.LoadScene("Construction Shop", LoadSceneMode.Single);
    }

    public void goToFactory()
    {
        SceneManager.LoadScene("Factory", LoadSceneMode.Single);
    }
}
