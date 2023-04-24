using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_TurnToPlayerAndAttack : State
{
    public State_TurnToPlayerAndAttack(IStateInform _informTarget, EntityController _entityController)
    {
        informTarget = _informTarget;
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

        if (true == entityController.AI.PlayerInAttackRange)
        {
            entityController.AI.AttackPlayer();
        }
    }
}
