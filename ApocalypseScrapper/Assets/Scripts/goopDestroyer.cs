using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goopDestroyer : MonoBehaviour
{
    float timeAtInstation;
    [SerializeField] float timeToDestroyGoop;
    // Start is called before the first frame update
    void Start()
    {
        timeAtInstation = Time.fixedTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.fixedTime - timeAtInstation > timeToDestroyGoop)
        {
            Destroy(gameObject);
        }
    }
}
