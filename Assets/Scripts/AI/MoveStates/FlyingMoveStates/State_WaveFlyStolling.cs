using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_WaveFlyStolling : State, IStateInform
{
    public State_WaveFlyStolling(IStateInform _informTarget, EntityController _entityController)
    {
        informTarget = _informTarget;
        entityController = _entityController;

        _WaveFlyState = new WaveFlyState(this, entityController);
        _WaveIdleState = new WaveIdleState(this, entityController);
    }

    private WaveFlyState _WaveFlyState;
    private WaveIdleState _WaveIdleState;

    private StateMachine stateMachine = new StateMachine();

    public float minMoveDist;
    public float maxMoveDist;

    public float minWaitTime;
    public float maxWaitTime;

    public float timeForFullWave
    {
        get { return _WaveFlyState.timeForFullWave; }
        set
        {
            _WaveFlyState.timeForFullWave = value;
            _WaveIdleState.timeForFullWave = value;
        }
    }

    public float waveScale
    {
        get { return _WaveFlyState.waveScale; }
        set
        {
            _WaveFlyState.waveScale = value;
            _WaveIdleState.waveScale = value;
        }

    }

    private Vector2 currentDirection = Vector2.zero;

    public void CollideWithGround(Vector2 collisionNormal)
    {
        if (Mathf.Abs(collisionNormal.x) >= 0.5f)
        {
            currentDirection.x *= -1;
            entityController.Flip();
        }
        
        currentDirection.y *= Mathf.Abs(collisionNormal.y) >= 0.5f ? -1 : 1;

        _WaveFlyState.SetNormalizedDirection(currentDirection);
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
        _WaveIdleState.TimeToWait = Random.Range(minWaitTime, maxWaitTime);

        stateMachine.SetState(_WaveIdleState);
    }

    private void SetMoveState()
    {
        _WaveFlyState.distance = Random.Range(minMoveDist, maxMoveDist);

        float angle = Random.Range(0, 2 * Mathf.PI);

        Vector2 newDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

        if (Mathf.Sign(newDirection.x) != Mathf.Sign(currentDirection.x))
        {          
            entityController.Flip();
        }

        currentDirection = newDirection;

        _WaveFlyState.SetNormalizedDirection(currentDirection);

        stateMachine.SetState(_WaveFlyState);
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
