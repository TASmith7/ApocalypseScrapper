using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainMenuManager : MonoBehaviour
{
    public static mainMenuManager instance;

    [Header("----- Menus -----")]
    public GameObject mainMenu;
    public GameObject loadLevelMenu;


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CueLoadLevelMenu()
    {
        mainMenu.SetActive(false);
        loadLevelMenu.SetActive(true);
    }

    public void CueMainMenuFromLoadLevel()
    {
        loadLevelMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
}
