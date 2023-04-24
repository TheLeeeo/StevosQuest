using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_RandomStroll : State, IStateInform
{
    public State_RandomStroll(IStateInform _informTarget_, EntityController _entityController_)
    {
        informTarget = _informTarget_;
        entityController = _entityController_;

        _MoveXState = new MoveXState(this, entityController);
        _WaitForTimeState = new State_WaitForTime(this);
    }

    private MoveXState _MoveXState;
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
        _WaitForTimeState.TimeToWait = Random.Range(MinWaitTime, MaxWaitTime);

        stateMachine.SetState(_WaitForTimeState);
    }

    private void SetMoveState()
    {
        _MoveXState.distance = Random.Range(MinMoveDist, MaxMoveDist);
        int newDirection = RandomNumbers.RandomFromArgs(-1, 1);

        if (newDirection != currentDirection)
        {
            currentDirection = newDirection;

            entityController.Flip();
        }

        _MoveXState.SetDirection(currentDirection);

        stateMachine.SetState(_MoveXState);
    }

    public override void Inform(InformEnum passedInfo)
    {
        switch (passedInfo)
        {
            case InformEnum.MovementCompleted:
                entityController.entityRigidbody.velocity = Vector2.zero;
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
