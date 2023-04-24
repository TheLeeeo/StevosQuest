using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_FlyToPlayerAndAttack : State
{
    public State_FlyToPlayerAndAttack(IStateInform _informTarget, EntityController _entityController)
    {
        informTarget = _informTarget;
        entityController = _entityController;

        _FlyState = new State_Fly(this, entityController);
    }

    private State_Fly _FlyState;

    private StateMachine stateMachine = new StateMachine();

    private Vector2 currentDirection;

    public override void StateEnter()
    {
        _FlyState.distance = int.MaxValue;
        stateMachine.SetState(_FlyState);

        currentDirection.x = entityController.isFacingRight ? 1 : -1;
    }

    public override void StateUpdate()
    {
        if (true == entityController.AI.PlayerInAttackRange)
        {
            entityController.AI.AttackPlayer();
        }
        else
        {
            Vector2 newDirection = PlayerController._instance.entityRigidbody.position - entityController.entityRigidbody.position;

            if (Mathf.Sign(newDirection.x) != Mathf.Sign(currentDirection.x))
            {
                entityController.Flip();
            }

            currentDirection = newDirection;

            _FlyState.SetNotNormalDirection(newDirection);

            stateMachine.Update();
        }
    }
}
