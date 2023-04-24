using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryShroomAI : AIBase
{
    #region AnimatorHashes
    public static int anim_AwakeHash;

    static AngryShroomAI()
    {
        anim_AwakeHash = Animator.StringToHash("IsAwake");
    }
    #endregion

    #region StateSetup
    private State_TurnToPlayerAndAttack _TurnToPlayerAndAttackState;

    private State_Null _NullState = new State_Null();
    private State_TimeAfterPlayerLost _TimeAfterPlayerLostState;
    #endregion

    private StateMachine aggressionStateMachine = new StateMachine();

    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField]
    private Transform firePoint;

    [SerializeField]
    private float maxShootingDistance;

    [SerializeField]
    private int shootingSpeed;

    [SerializeField]
    private int baseDamage;

    [SerializeField]
    private float attackTime;
    private float timeOfLastAttack = 0;

    public override void AggrevateOnPlayer()
    {
        if (false == targetingPlayer)
        {
            base.AggrevateOnPlayer();

            behaviourStateMachine.SetState(_TurnToPlayerAndAttackState);
            targetingPlayer = true;
            entityController.animator.SetBool(anim_AwakeHash, true);
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

            aggressionStateMachine.SetState(_TimeAfterPlayerLostState);
            entityController.animator.SetBool(anim_AwakeHash, true);
        }
    }

    public override void AttackPlayer()
    {
        if (Time.time - timeOfLastAttack >= attackTime)
        {
            entityController.soundController.PlayAttackSound();

            ShootSporeBall();

            timeOfLastAttack = Time.time;
        }
    }

    private void ShootSporeBall()
    {
        GameObject go = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        Vector2 velocity = (PlayerController._instance.playerTransform.position - transform.position).normalized * shootingSpeed;
        if (velocity.y < 0.1f)
        {
            velocity.y = 0.1f;
        }

        go.GetComponent<FlyingBoulderController>().Initiate(velocity, baseDamage);
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

    private void Start()
    {
        _TurnToPlayerAndAttackState = new State_TurnToPlayerAndAttack(this, entityController);
        _TimeAfterPlayerLostState = new State_TimeAfterPlayerLost(this, 5);

        behaviourStateMachine.SetState(_NullState);
        aggressionStateMachine.SetState(_NullState);
    }

    private void FixedUpdate()
    {
        behaviourStateMachine.Update();
        aggressionStateMachine.Update();
    }

    public override void Inform(InformEnum stateInfo)
    {
        switch (stateInfo)
        {
            case InformEnum.OutOfRangeTimer:
                behaviourStateMachine.SetState(_NullState);
                aggressionStateMachine.SetState(_NullState);
                targetingPlayer = false;
                entityController.animator.SetBool(anim_AwakeHash, false);
                break;

            default:
                InvalidInform.Error(stateInfo, this);
                break;

        }
    }
}
