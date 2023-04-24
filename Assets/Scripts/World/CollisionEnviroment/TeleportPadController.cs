using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPadController : MonoBehaviour
{
    private static bool playerBeenTeleported; //So that the player does not get teleported back and forwards

    public static GameObject sourcePad;

    [SerializeField]
    private Transform connectedPad;

    private void OnTriggerEnter2D()
    {
        if (playerBeenTeleported == false)
        {
            PlayerController._instance.transform.position = new Vector3(connectedPad.position.x, connectedPad.position.y + 1, 0);
            playerBeenTeleported = true;
            sourcePad = gameObject;
        }
    }

    private void OnTriggerExit2D()
    {
        if (gameObject != sourcePad)
        {
            playerBeenTeleported = false;
        }
    }
}
