using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class salvageableObject : MonoBehaviour, ISalvageable
{
    [SerializeField] public float salvageValue;
    public float salvageTime;

   void Start()
    {
        salvageTime = salvageValue / 1000.0f;
        if (salvageTime < 0.3f)
            salvageTime = 0.3f;
    }

    void Update()
    {

    }

    //public void SalvageObject(GameObject objectToSalvage) { }
    

}
