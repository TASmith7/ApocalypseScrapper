using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ShieldStats : ScriptableObject
{
    public int shieldHealth;
    public float damageBlocked;
    public int blockRadius;
    public GameObject model;
    public AudioClip pickupSound;
    public AudioClip blockSound;
    public AudioClip breakSound;
}
