using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhooperAI : AIBase
{
    private State_FlyStolling StrollState; //Idle
    private State_FlyToPlayerAndAttack _FlyToPlayerAndAttackState; //Attack

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
    private GameObject FartObjectPrefab;

    [SerializeField]
    private float CloudSpeed;

    private Vector3 AttackAnimSquish = new Vector3(1.2f, 0.8f, 0);
    private Vector3 NormalScale = new Vector3(1f, 1f, 0);
    private const float AnimTime = 0.2f;

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

        _FlyToPlayerAndAttackState = new State_FlyToPlayerAndAttack(this, entityController);

        aggressionStateMachine.SetState(_NullState);
        behaviourStateMachine.SetState(StrollState);
    }

    public override void AggrevateOnPlayer()
    {
        if (false == targetingPlayer)
        {
            base.AggrevateOnPlayer();

            behaviourStateMachine.SetState(_FlyToPlayerAndAttackState);
            targetingPlayer = true;
        }
    }

    private void PerformAttack()
    {
        GasCloudController gasCloudController = Instantiate(FartObjectPrefab, transform.position, Quaternion.identity).GetComponent<GasCloudController>();
        gasCloudController.Initiate((PlayerController._instance.transform.position - transform.position).normalized * CloudSpeed);
        NormalScale.x = entityController.isFacingRight ? 1 : -1;

        entityController.soundController.PlayAttackSound();

        LeanTween.scale(gameObject, NormalScale, AnimTime);
    }


    public override void AttackPlayer()
    {
        if (Time.time - timeOfLastAttack >= attackTime)
        {
            AttackAnimSquish.x = entityController.isFacingRight ? 1.2f : -1.2f;
            LeanTween.scale(gameObject, AttackAnimSquish, AnimTime).setOnComplete(PerformAttack);

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
            case InformEnum.OutOfRangeTimer:
                behaviourStateMachine.SetState(StrollState);
                aggressionStateMachine.SetState(_NullState);
                targetingPlayer = false;
                break;

            default:
                InvalidInform.Error(stateInfo, this);
                break;

        }
    }
}
