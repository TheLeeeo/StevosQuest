using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_WaveFlyToPlayerAndAttack : State
{
    public State_WaveFlyToPlayerAndAttack(IStateInform _informTarget, EntityController _entityController)
    {
        informTarget = _informTarget;
        entityController = _entityController;

        _WaveFlyState = new WaveFlyState(this, entityController);
    }

    private WaveFlyState _WaveFlyState;

    public float timeForFullWave
    {
        get { return _WaveFlyState.timeForFullWave; }
        set
        {
            _WaveFlyState.timeForFullWave = value;
        }
    }

    public float waveScale
    {
        get { return _WaveFlyState.waveScale; }
        set
        {
            _WaveFlyState.waveScale = value;
        }

    }

    private StateMachine stateMachine = new StateMachine();

    private Vector2 currentDirection;

    public override void StateEnter()
    {
        _WaveFlyState.distance = int.MaxValue;
        stateMachine.SetState(_WaveFlyState);

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

            _WaveFlyState.SetNotNormalDirection(newDirection);

            stateMachine.Update();
        }
    }
}
