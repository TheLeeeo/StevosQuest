using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_RunToEntityX : State
{
    public State_RunToEntityX(IStateInform _informTarget, EntityController _entityController)
    {
        informTarget = _informTarget;
        entityController = _entityController;

        _RunXState = new RunXState(this, entityController);
    }

    private RunXState _RunXState;

    private EntityController entityToMatch;

    private StateMachine stateMachine = new StateMachine();

    private int currentDirection;

    /// <summary>
    /// The distance attempted to reach
    /// </summary>
    private float SquaredTargetRange;

    public void SetTargetRange(float distance)
    {
        SquaredTargetRange = distance * distance;
    }

    public void SetTargetEntity(EntityController newEntity)
    {
        entityToMatch = newEntity;
    }

    public override void StateEnter()
    {
        _RunXState.distance = int.MaxValue;
        stateMachine.SetState(_RunXState);

        currentDirection = entityController.isFacingRight ? 1 : -1;
    }

    public override void StateUpdate()
    {
        //Debug.Log((entityToMatch.entityRigidbody.position - entityController.entityRigidbody.position).sqrMagnitude);
        if ((entityToMatch.entityRigidbody.position - entityController.entityRigidbody.position).sqrMagnitude <= SquaredTargetRange) 
        {
            informTarget.Inform(InformEnum.ReachedTargetRange);
        }
        else
        {
            int newDirection = (int)Mathf.Sign(entityToMatch.entityRigidbody.position.x - entityController.entityRigidbody.position.x);
            if (newDirection != currentDirection)
            {
                entityController.Flip();

                currentDirection = newDirection;

                _RunXState.SetDirection(newDirection);
            }

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
