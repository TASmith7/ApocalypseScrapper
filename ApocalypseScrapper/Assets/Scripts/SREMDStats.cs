using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class SREMDStats : ScriptableObject
{
    public int sremdHealth;
    public float sremdBlocked;
    public int sremdRadius;
    public GameObject model;
    public AudioClip pickupSound;
    public AudioClip throwSound;
    public AudioClip explodeSound;
}