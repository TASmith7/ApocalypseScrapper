using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public Transform player;

    private void LateUpdate()
    {
        //Follow Player
        Vector3 newPOS = player.position;
        newPOS.y = gameManager.instance.player.transform.position.y+120;
        transform.position = newPOS;

        //Rotate with Player
        transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
    }
}
