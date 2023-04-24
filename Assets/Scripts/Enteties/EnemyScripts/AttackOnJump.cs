using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackOnJump : MonoBehaviour
{
    [SerializeField]
    private EntityController entityController;

    private bool HasDamagedThisJump;

    public void ResetJumpCheck()
    {
        HasDamagedThisJump = false;
    }

    private void OnTriggerStay2D()
    {
        if (!HasDamagedThisJump && !entityController.isGrounded)
        {
            HasDamagedThisJump = true;
            entityController.AI.AttackPlayer();
        }
    }
}
