using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_FlyStolling : State
{
    public State_FlyStolling(IStateInform _informTarget, EntityController _entityController)
    {
        informTarget = _informTarget;
        entityController = _entityController;

        _FlyState = new State_Fly(this, entityController);
        _IdleState = new State_WaitForTime(this);
    }

    private State_Fly _FlyState;
    private State_WaitForTime _IdleState;

    private StateMachine stateMachine = new StateMachine();

    public float minMoveDist;
    public float maxMoveDist;

    public float minWaitTime;
    public float maxWaitTime;


    private Vector2 currentDirection = Vector2.zero;

    public void CollideWithGround(Vector2 collisionNormal)
    {
        if (Mathf.Abs(collisionNormal.x) >= 0.5f)
        {
            currentDirection.x *= -1;
            entityController.Flip();
        }

        currentDirection.y *= Mathf.Abs(collisionNormal.y) >= 0.5f ? -1 : 1;

        _FlyState.SetNormalizedDirection(currentDirection);
    }

    public override void StateEnter()
    {
        SetWaitState();

        currentDirection.x = entityController.isFacingRight ? 1 : -1;
    }

    public override void StateUpdate()
    {
        stateMachine.Update();
    }

    private void SetWaitState()
    {
        _IdleState.TimeToWait = Random.Range(minWaitTime, maxWaitTime);

        stateMachine.SetState(_IdleState);
    }

    private void SetMoveState()
    {
        _FlyState.distance = Random.Range(minMoveDist, maxMoveDist);

        float angle = Random.Range(0, 2 * Mathf.PI);

        Vector2 newDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

        if (Mathf.Sign(newDirection.x) != Mathf.Sign(currentDirection.x))
        {
            entityController.Flip();
        }

        currentDirection = newDirection;

        _FlyState.SetNormalizedDirection(currentDirection);

        stateMachine.SetState(_FlyState);
    }

    public override void Inform(InformEnum passedInfo)
    {
        switch (passedInfo)
        {
            case InformEnum.Flight_MovementCompleted:
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
