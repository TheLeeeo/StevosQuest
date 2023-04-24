using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_RandomJumpStrollingWithPause : State
{
    public State_RandomJumpStrollingWithPause(IStateInform _informTarget_, EntityController _entityController_)
    {
        informTarget = _informTarget_;
        entityController = _entityController_;

        _JumpXState = new State_JumpXWithPause(this, entityController);
        _WaitForTimeState = new State_WaitForTime(this);
    }

    private State_JumpXWithPause _JumpXState;
    private State_WaitForTime _WaitForTimeState;

    private StateMachine stateMachine = new StateMachine();

    public float MinMoveDist;
    public float MaxMoveDist;

    public float MinWaitTime;
    public float MaxWaitTime;

    public float JumpDistance;
    public float JumpHeight;

    private int currentDirection = 1;

    public float jumpWaitTime;

    public override void StateEnter()
    {
        SetWaitState();

        currentDirection = entityController.isFacingRight ? 1 : -1;

        _JumpXState.jumpDistance = this.JumpDistance;
        _JumpXState.jumpHeight = this.JumpHeight;

        _JumpXState.jumpWaitTime = this.jumpWaitTime;
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
        _JumpXState.distance = Random.Range(MinMoveDist, MaxMoveDist);
        int newDirection = RandomNumbers.RandomFromArgs(-1, 1);

        if (newDirection != currentDirection)
        {
            currentDirection = newDirection;

            entityController.Flip();

            _JumpXState.SetDirection(currentDirection);
        }

        stateMachine.SetState(_JumpXState);
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

            case InformEnum.ReachedWall:
                entityController.entityRigidbody.velocity = Vector2.zero;
                SetWaitState();
                break;

            default:
                InvalidInform.Error(passedInfo, this);
                break;

        }
    }
}
