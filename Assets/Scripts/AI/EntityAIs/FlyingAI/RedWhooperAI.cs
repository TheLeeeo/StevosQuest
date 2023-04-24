using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedWhooperAI : AIBase
{
    private State_FlyStolling StrollState; //Idle
    private State_FlyFollowTransform _FollowTransformState; //Attack

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

    private StateMachine aggressionStateMachine = new StateMachine();

    [SerializeField]
    private float attackTime;
    private float timeOfLastAttack = 0;

    [SerializeField]
    private float followDistance;

    [SerializeField]
    private Effect effectToGive;

    [SerializeField]
    private ParticleSystem[] fireParticles;

    private void Start()
    {
        StrollState = new State_FlyStolling(this, entityController)
        {
            maxMoveDist = this.MaxMoveDist,
            minMoveDist = this.MinMoveDist,
            maxWaitTime = this.MaxWaitTime,
            minWaitTime = this.MinWaitTime,
        };

        _TimeAfterPlayerLostState = new State_TimeAfterPlayerLost(this, 5);

        _FollowTransformState = new State_FlyFollowTransform(this, entityController) { target = PlayerController._instance.transform, followDistance = this.followDistance};

        aggressionStateMachine.SetState(_NullState);
        behaviourStateMachine.SetState(StrollState);
    }

    public override void AggrevateOnPlayer()
    {
        if (false == targetingPlayer)
        {
            base.AggrevateOnPlayer();

            behaviourStateMachine.SetState(_FollowTransformState);
            targetingPlayer = true;

            foreach (ParticleSystem particleSystem in fireParticles)
            {
                particleSystem.Play();
            }

            entityController.soundController.PlayAttackSound();
        }
    }


    public override void AttackPlayer()
    {
        if (Time.time - timeOfLastAttack >= attackTime)
        {
            PlayerHealth._instance.GiveEffect(effectToGive);

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

            behaviourStateMachine.SetState(StrollState);
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
        if (behaviourStateMachine.CurrentState == StrollState)
        {
            StrollState.CollideWithGround(collision.contacts[0].normal); //possible bug if two different contacts with different normals (tested and never observed)
        }        
    }

    public override void Inform(InformEnum stateInfo)
    {
        switch (stateInfo)
        {
            case InformEnum.OutOfRangeTimer: //stop aggro
                behaviourStateMachine.SetState(StrollState);
                aggressionStateMachine.SetState(_NullState);

                foreach (ParticleSystem particleSystem in fireParticles)
                {
                    particleSystem.Stop();
                }

                entityController.soundController.Stop();

                targetingPlayer = false;
                break;

            default:
                InvalidInform.Error(stateInfo, this);
                break;

        }
    }
}
