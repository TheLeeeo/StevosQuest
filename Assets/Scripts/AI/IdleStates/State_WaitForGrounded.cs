using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_WaitForGrounded : State
{
    public State_WaitForGrounded(IStateInform _informTarget_, EntityController _entityController_)
    {
        informTarget = _informTarget_;
        entityController = _entityController_;
    }

    public override void StateUpdate()
    {
        if (GroundCheck.CheckForGround(entityController.AI.groundCollider))
        {
            entityController.entityRigidbody.velocity = Vector2.zero;
            informTarget.Inform(InformEnum.GotGrounded);
        }
    }
}
