using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomStrollToEdge : State
{
    public RandomStrollToEdge(IStateInform _informTarget_, EntityController _entityController_)
    {
        informTarget = _informTarget_;
        entityController = _entityController_;

        _MoveState = new MoveToEdge(this, entityController);
        _WaitForTimeState = new State_WaitForTime(this);
    }

    private MoveToEdge _MoveState;
    private State_WaitForTime _WaitForTimeState;

    private StateMachine stateMachine = new StateMachine();

    public float MinMoveDist;
    public float MaxMoveDist;

    public float MinWaitTime;
    public float MaxWaitTime;

    private int currentDirection = 1;

    public override void StateEnter()
    {
        SetWaitState();

        currentDirection = entityController.isFacingRight ? 1 : -1;
    }

    public override void StateUpdate()
    {
        stateMachine.Update();
    }

    private void SetWaitState()
    {
        entityController.entityRigidbody.velocity = Vector2.zero;

        _WaitForTimeState.TimeToWait = Random.Range(MinWaitTime, MaxWaitTime);

        stateMachine.SetState(_WaitForTimeState);
    }

    private void SetMoveState()
    {
        _MoveState.distanceLeft = Random.Range(MinMoveDist, MaxMoveDist);     

        currentDirection *= -1;

        int LookDirection = entityController.isFacingRight ? 1 : -1;

        if(currentDirection != LookDirection)
        {
            entityController.Flip();
        }        

        _MoveState.SetDirection(currentDirection);

        stateMachine.SetState(_MoveState);
    }

    public override void Inform(InformEnum passedInfo)
    {
        switch (passedInfo)
        {
            case InformEnum.MovementCompleted:                
                SetWaitState();
                break;

            case InformEnum.ReachedWall:
                entityController.Flip();
                SetWaitState();
                break;

            case InformEnum.ReachedEdge:
                SetWaitState();
                break;

            case InformEnum.TimerFinished:
                SetMoveState();
                break;

            default:
                InvalidInform.Error(passedInfo, this);
                break;

        }
    }
}
