using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpToNodeState : State
{
    public JumpToNodeState(IStateInform _informTarget_, EntityController _entityController_)
    {
        informTarget = _informTarget_;
        entityController = _entityController_;
    }

    public void SetTarget(IntVector2 nodeAdress)
    {
        jumpTarget = nodeAdress;
    }

    private IntVector2 jumpTarget;

    private bool hasLeftGround = false;

    private JumpParameters jumpParameters;

    public override void StateEnter()
    {
        jumpParameters = JumpParameters.ParametersTo(jumpTarget, entityController);

        if (JumpParameters.Zero == jumpParameters)
        {
            informTarget.Inform(InformEnum.JumpUnavaliable);
        }
        else
        {
            entityController.entityRigidbody.velocity = new Vector2(jumpParameters.Speed * Mathf.Sign(jumpTarget.x - entityController.entityRigidbody.position.x), jumpParameters.JumpForce);
            entityController.isGrounded = false;
            entityController.Jump();
        }
    }

    public override void StateUpdate()
    {
        entityController.entityRigidbody.velocity = new Vector2(jumpParameters.Speed * Mathf.Sign(jumpTarget.x - entityController.entityRigidbody.position.x), entityController.entityRigidbody.velocity.y);

        if (GroundCheck.CheckForGround(entityController.AI.groundCollider)) //Is grounded
        {
            if (hasLeftGround)
            {
                entityController.entityRigidbody.velocity = Vector2.zero;
                entityController.isGrounded = true;
                informTarget.Inform(InformEnum.JumpCompleted);
            }
        }
        else
        {
            hasLeftGround = true;
        }
    }

    public override void StateExit()
    {
        hasLeftGround = false;
    }
}
