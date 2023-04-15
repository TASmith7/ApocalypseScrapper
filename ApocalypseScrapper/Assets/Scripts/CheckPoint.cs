using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] Renderer model;
    [SerializeField] GameObject triggerEffect;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (gameManager.instance.playerSpawnPos.transform.position != transform.position)
            {
                gameManager.instance.playerSpawnPos.transform.position = transform.position;
                if (triggerEffect)
                {
                    Instantiate(triggerEffect, transform.position, triggerEffect.transform.rotation);
                }
                StartCoroutine(Flash());
            }


        }
    }
    IEnumerator Flash()
    {
        model.material.color = Color.red;
        gameManager.instance.checkpointMenu.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        model.material.color = Color.red;

        yield return new WaitForSeconds(0.2f);
        model.material.color = Color.blue;

        yield return new WaitForSeconds(0.2f);
        model.material.color = Color.yellow;

        yield return new WaitForSeconds(0.2f);
        model.material.color = Color.magenta;

        yield return new WaitForSeconds(0.2f);
        model.material.color = Color.cyan;

        yield return new WaitForSeconds(0.2f);
        model.material.color = Color.white;

        yield return new WaitForSeconds(0.2f);
        model.material.color = Color.black;

        yield return new WaitForSeconds(0.2f);
        model.material.color = Color.gray;

        yield return new WaitForSeconds(0.2f);
        model.material.color = Color.green;

        yield return new WaitForSeconds(0.2f);
        model.material.color = Color.red;

        yield return new WaitForSeconds(0.2f);
        model.material.color = Color.blue;

        yield return new WaitForSeconds(0.2f);
        model.material.color = Color.yellow;

        yield return new WaitForSeconds(0.2f);
        model.material.color = Color.magenta;

        yield return new WaitForSeconds(0.2f);
        model.material.color = Color.cyan;

        yield return new WaitForSeconds(0.2f);
        model.material.color = Color.white;

        yield return new WaitForSeconds(0.2f);
        model.material.color = Color.black;

        yield return new WaitForSeconds(0.2f);
        model.material.color = Color.gray;

        yield return new WaitForSeconds(0.2f);
        model.material.color = Color.green;

        yield return new WaitForSeconds(0.2f);
        model.material.color = Color.red;

        yield return new WaitForSeconds(0.2f);
        model.material.color = Color.blue;
        gameManager.instance.checkpointMenu.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);

        model.material.color = Color.white;
    }
}
