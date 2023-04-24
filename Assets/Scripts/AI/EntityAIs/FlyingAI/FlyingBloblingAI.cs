using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingBloblingAI : AIBase
{
    private State_WaveFlyStolling _WaveStrollState; //Idle
    private State_WaveFlyToPlayerAndAttack _WaveFlyToPlayerAndAttackState; //Attack

    private State_Null _NullState = new State_Null();
    private State_TimeAfterPlayerLost _TimeAfterPlayerLostState;

    [SerializeField]
    private float MaxMoveDist;
    [SerializeField]
    private float MinMoveDist;
    [SerializeField]
    private float MaxWaitTime;
    [SerializeField]
    private float MinWaitTime;

    [SerializeField]
    private float TimeForFullWave;
    [SerializeField]
    private float WaveScale;

    private StateMachine aggressionStateMachine = new StateMachine();

    [SerializeField]
    private float attackTime;
    private float timeOfLastAttack = 0;

    [SerializeField]
    private int baseDamage;

    private void Start()
    {
        _WaveStrollState = new State_WaveFlyStolling(this, entityController)
        {
            maxMoveDist = this.MaxMoveDist,
            minMoveDist = this.MinMoveDist,
            maxWaitTime = this.MaxWaitTime,
            minWaitTime = this.MinWaitTime,
            timeForFullWave = this.TimeForFullWave,
            waveScale = this.WaveScale
        };

        _TimeAfterPlayerLostState = new State_TimeAfterPlayerLost(this, 5);

        _WaveFlyToPlayerAndAttackState = new State_WaveFlyToPlayerAndAttack(this, entityController)
        {
            timeForFullWave = this.TimeForFullWave,
            waveScale = this.WaveScale
        };

        aggressionStateMachine.SetState(_NullState);
        behaviourStateMachine.SetState(_WaveStrollState);
    }

    public override void AggrevateOnPlayer()
    {
        if (false == targetingPlayer)
        {
            base.AggrevateOnPlayer();

            behaviourStateMachine.SetState(_WaveFlyToPlayerAndAttackState);
            targetingPlayer = true;
        }
    }

    public override void AttackPlayer()
    {
        if (Time.time - timeOfLastAttack >= attackTime)
        {
            entityController.soundController.PlayAttackSound();

            PlayerHealth._instance.Damage((int)(baseDamage * LevelData.GetEntityPowerMultiplier()));
            timeOfLastAttack = Time.time;
        }
    }

    public override void NotifyOfAttack(int attackDirection)
    {
        if (false == targetingPlayer)
        {
            if ((entityController.isFacingRight && -1 == attackDirection) || (!entityController.isFacingRight && 1 == attackDirection)) //looking in the wrong direction
            {
                entityController.Flip();
            }

            behaviourStateMachine.SetState(_WaveStrollState);
        }
    }

    public override void PlayerEnteredDetection()
    {
        AggrevateOnPlayer();

        if (true == targetingPlayer) //If the player re-entered the sight when the outOfRangeTimer is running
        {
            aggressionStateMachine.SetState(_NullState);
        }
    }

    public override void PlayerLeftDetection()
    {
        aggressionStateMachine.SetState(_TimeAfterPlayerLostState);
    }

    public void FixedUpdate()
    {
        behaviourStateMachine.Update();
        aggressionStateMachine.Update();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (behaviourStateMachine.CurrentState == _WaveStrollState)
        {
            _WaveStrollState.CollideWithGround(collision.contacts[0].normal); //possible bug if two different contacts with different normals (tested and never observed)
        }
    }

    public override void Inform(InformEnum stateInfo)
    {
        switch (stateInfo)
        {
            case InformEnum.OutOfRangeTimer:
                behaviourStateMachine.SetState(_WaveStrollState);
                aggressionStateMachine.SetState(_NullState);
                targetingPlayer = false;
                break;

            default:
                InvalidInform.Error(stateInfo, this);
                break;

        }
    }
}
