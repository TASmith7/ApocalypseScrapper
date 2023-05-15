using System.Collections;
using UnityEngine;

public class PlayerCampStopper : MonoBehaviour
{
    public int hurtAmt;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(HurtCamper());

        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopCoroutine(HurtCamper());

        }
    }
    IEnumerator HurtCamper()
    {
        yield return new WaitForSeconds(2);
        gameManager.instance.playerScript.TakeDamage(hurtAmt);
    }
}
