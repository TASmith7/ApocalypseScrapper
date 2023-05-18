using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mainMenuManager : MonoBehaviour
{
    public static mainMenuManager instance;

    [Header("----- Menus -----")]
    public GameObject mainMenu;
    public GameObject loadLevelMenu;

    [Header("----- Load Level Buttons -----")]
    public Button lvl1;
    public Button lvl2;
    public Button lvl3;
    public Button bossLvl;

    [Header("----- Level Buttons Unavailable Messsages -----")]
    public GameObject lvl2ButtonUnavailable;
    public GameObject lvl3ButtonUnavailable;
    public GameObject bossLvlButtonUnavailable;

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
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        mainMenu.SetActive(true);
        loadLevelMenu.SetActive(false);

        if(PlayerPrefs.HasKey("BeatLevel1"))
        {
            if(PlayerPrefs.GetInt("BeatLevel1") == 0)
            {
                lvl2.interactable = false;
                lvl2ButtonUnavailable.SetActive(true);
            }
            else
            {
                lvl2.interactable = true;
                lvl2ButtonUnavailable.SetActive(false);
            }
        }
        else
        {
            lvl2.interactable = false;
            lvl2ButtonUnavailable.SetActive(true);
        }

        if (PlayerPrefs.HasKey("BeatLevel2"))
        {
            if (PlayerPrefs.GetInt("BeatLevel2") == 0)
            {
                lvl3.interactable = false;
                lvl3ButtonUnavailable.SetActive(true);
            }
            else
            {
                lvl3.interactable = true;
                lvl3ButtonUnavailable.SetActive(false);
            }
        }
        else
        {
            lvl3.interactable = false;
            lvl3ButtonUnavailable.SetActive(true);
        }

        if (PlayerPrefs.HasKey("BeatLevel3"))
        {
            if (PlayerPrefs.GetInt("BeatLevel3") == 0)
            {
                bossLvl.interactable = false;
                bossLvlButtonUnavailable.SetActive(true);
            }
            else
            {
                bossLvl.interactable = true;
                bossLvlButtonUnavailable.SetActive(false);
            }
        }
        else
        {
            bossLvl.interactable = false;
            bossLvlButtonUnavailable.SetActive(true);
        }

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
