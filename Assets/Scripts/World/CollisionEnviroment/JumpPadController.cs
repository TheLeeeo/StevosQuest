using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPadController : MonoBehaviour
{
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position + new Vector3(-0.5f, JumpHeight), transform.position + new Vector3(0.5f, JumpHeight));
    }

    private bool playerJustJumped; //so that the jump boost is only applied once per jump

    [SerializeField]
    private float JumpHeight;

    private float JumpForce;

    private void Start()
    {
        JumpForce = 10 * Mathf.Sqrt(0.2f * (JumpHeight + 0.25f) * PlayerController._instance.entityRigidbody.gravityScale);
    }

    private void OnTriggerEnter2D()
    {
        if(playerJustJumped == false)
        {
            PlayerMovement._instance.Jump(JumpForce);
            playerJustJumped = true;
        }
    }

    private void OnTriggerExit2D()
    {
        playerJustJumped = false;
    }
}
