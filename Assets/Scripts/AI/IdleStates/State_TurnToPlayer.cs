using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_TurnToPlayer : State
{
    public State_TurnToPlayer(EntityController _entityController)
    {
        entityController = _entityController;
    }

    private int currentDirection;

    public override void StateEnter()
    {
        currentDirection = entityController.isFacingRight ? 1 : -1;
    }

    public override void StateUpdate()
    {
        int newDirection = (int)Mathf.Sign(PlayerController._instance.entityRigidbody.position.x - entityController.entityRigidbody.position.x);
        if (newDirection != currentDirection)
        {
            entityController.Flip();

            currentDirection = newDirection;
        }
    }
}
