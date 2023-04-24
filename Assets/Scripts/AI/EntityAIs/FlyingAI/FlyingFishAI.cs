using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingFishAI : AIBase
{
    #region AnimatorHashes
    public static int anim_IsFlappingHash;

    static FlyingFishAI()
    {
        anim_IsFlappingHash = Animator.StringToHash("IsFlapping");
    }
    #endregion

    private State_FlyRandomDiagonal _RandomDiagonalState; //Idle
    private State_DiagonalFlyToPlayerAndAttack _FlyToPlayerAndAttackState; //Attack

    private State_Null _NullState = new State_Null();
    private State_TimeAfterPlayerLost _TimeAfterPlayerLostState;

    [SerializeField]
    private float MaxMoveDist;
    [SerializeField]
    private float MinMoveDist;
    [SerializeField]
    private float AttackMoveDist;

    [SerializeField]
    [Tooltip("Degrees you cunt")]
    private float MaxAngle;

    private StateMachine aggressionStateMachine = new StateMachine();

    [SerializeField]
    private float attackTime;
    private float timeOfLastAttack = 0;

    [SerializeField]
    private int baseDamage;

    private void Start()
    {
        _RandomDiagonalState = new State_FlyRandomDiagonal(this, entityController)
        {
            maxAngle = this.MaxAngle,
            minMoveDist = MinMoveDist,
            maxMoveDist = MaxMoveDist
        };

        _TimeAfterPlayerLostState = new State_TimeAfterPlayerLost(this, 1);

        _FlyToPlayerAndAttackState = new State_DiagonalFlyToPlayerAndAttack(this, entityController)
        {
            maxAngle = this.MaxAngle,
            moveDist = this.AttackMoveDist
        };

        aggressionStateMachine.SetState(_NullState);
        behaviourStateMachine.SetState(_RandomDiagonalState);
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

    public override void AttackPlayer()
    {
        if (Time.time - timeOfLastAttack >= attackTime)
        {
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

            behaviourStateMachine.SetState(_RandomDiagonalState);
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
        entityController.animator.SetBool(anim_IsFlappingHash, entityController.entityRigidbody.velocity.y >= 0);

        behaviourStateMachine.Update();
        aggressionStateMachine.Update();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (behaviourStateMachine.CurrentState == _RandomDiagonalState)
        {
            _RandomDiagonalState.CollideWithGround(collision.contacts[0].normal); //possible bug if two different contacts with different normals (tested and never observed)
        }
    }

    public override void Inform(InformEnum stateInfo)
    {
        switch (stateInfo)
        {           
            case InformEnum.OutOfRangeTimer:
                behaviourStateMachine.SetState(_RandomDiagonalState);
                aggressionStateMachine.SetState(_NullState);
                targetingPlayer = false;
                break;

            default:
                InvalidInform.Error(stateInfo, this);
                break;

        }
    }
}
