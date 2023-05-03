using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class salvageableObject : MonoBehaviour, ISalvageable
{
    [SerializeField] public float salvageValue;
    public float salvageTime;

   void Start()
    { 
        gameObject.tag = "Salvage";
        salvageTime = salvageValue / 1000.0f;
        if (salvageTime < 0.5f)
            salvageTime = 0.5f;
    }

    void Update()
    {

    }
}
