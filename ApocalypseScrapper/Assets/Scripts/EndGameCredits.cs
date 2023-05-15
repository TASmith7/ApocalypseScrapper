using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameCredits : MonoBehaviour
{
    [SerializeField] public GameObject blackScreenOverlay;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeSinceLevelLoad > 65)
        {
            blackScreenOverlay.SetActive(true);
        }

        if(Time.timeSinceLevelLoad > 71)
        {
            returnToMainMenu.ReturnToMainMenu();
        }
    }
}
