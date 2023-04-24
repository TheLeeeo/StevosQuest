using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_RunToPlayerAndAttack : State
{
    public State_RunToPlayerAndAttack(IStateInform _informTarget, EntityController _entityController)
    {
        informTarget = _informTarget;
        entityController = _entityController;

        _RunXState = new RunXState(this, entityController);
    }

    private RunXState _RunXState;

    private StateMachine stateMachine = new StateMachine();

    private int currentDirection;

    public override void StateEnter()
    {
        _RunXState.distance = int.MaxValue;
        stateMachine.SetState(_RunXState);

        currentDirection = entityController.isFacingRight ? 1 : -1;

        _RunXState.SetDirection(currentDirection);
    }

    public override void StateExit()
    {
        entityController.entityRigidbody.velocity = Vector2.zero;
        entityController.isMoving = false;
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

            entityController.isMoving = false;           
        }
        else
        {
            entityController.isMoving = true;            

            _RunXState.SetDirection(newDirection);

            stateMachine.Update();
        }
    }

    public override void Inform(InformEnum stateInfo)
    {
        switch (stateInfo)
        {            
            case InformEnum.MovementCompleted:
                break;

            default:
                InvalidInform.Error(stateInfo, this);
                break;
        }
    }
}
