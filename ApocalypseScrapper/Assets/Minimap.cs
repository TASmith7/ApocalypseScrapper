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
        newPOS.y = transform.position.y;
        transform.position = newPOS;

        //Rotate with Player
        transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
    }
}
