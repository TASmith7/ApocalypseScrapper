using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class salvageableObject : MonoBehaviour, ISalvageable
{
    [SerializeField] public float salvageValue;
    public float salvageTime;

   void Start()
    {
        salvageTime = salvageValue / 2000.0f;
    }

    void Update()
    {

    }

    //public void SalvageObject(GameObject objectToSalvage) { }
    

}
