using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumplerAI : AIBase
{
    public static int anim_Jump;
    public static int anim_Attack;
    public static int anim_IsTargeting;
    public static int anim_IsMoving;
    public static int anim_IsGrounded;

    static DumplerAI()
    {
        anim_Jump = Animator.StringToHash("Jump");
        anim_Attack = Animator.StringToHash("Attack");
        anim_IsTargeting = Animator.StringToHash("IsTargeting");
        anim_IsMoving = Animator.StringToHash("IsMoving");
        anim_IsGrounded = Animator.StringToHash("IsGrounded");
    }

    private State_RandomStroll _RandomStrollState;
    private State_TurnToPlayerAndAttack _TurnToPlayerAndAttackState;

    private State_Null _NullState = new State_Null();
    private State_TimeAfterPlayerLost _TimeAfterPlayerLostState;

    private StateMachine aggressionStateMachine = new StateMachine();

    [SerializeField]
    private float attackTime;
    private float timeOfLastAttack = 0;

    [SerializeField]
    private GameObject BoulderObjectPrefab;

    private void Start()
    {
        _RandomStrollState = new State_RandomStroll(this, entityController) { MinWaitTime = 2, MaxWaitTime = 6, MinMoveDist = 2, MaxMoveDist = 3 };
        _TimeAfterPlayerLostState = new State_TimeAfterPlayerLost(this, 5);
        _TurnToPlayerAndAttackState = new State_TurnToPlayerAndAttack(this, entityController);

        behaviourStateMachine.SetState(_RandomStrollState);
        aggressionStateMachine.SetState(_NullState);
    }

    private void FixedUpdate()
    {
        entityController.animator.SetBool(anim_IsMoving, entityController.isMoving);
        entityController.animator.SetBool(anim_IsGrounded, entityController.isGrounded);        

        behaviourStateMachine.Update();
        aggressionStateMachine.Update();
    }

    public void Jump()
    {
        entityController.animator.SetTrigger(anim_Jump);
    }

    public override void AggrevateOnPlayer()
    {
        if (false == targetingPlayer)
        {
            timeOfLastAttack = Time.time;

            base.AggrevateOnPlayer();

            behaviourStateMachine.SetState(_TurnToPlayerAndAttackState);
            entityController.animator.SetBool(anim_IsTargeting, true);
            targetingPlayer = true;
        }
    }

    public override void AttackPlayer()
    {
        if (Time.time - timeOfLastAttack >= attackTime)
        {
            entityController.soundController.PlayAttackSound();

            RollingBoulderController boulderController = Instantiate(BoulderObjectPrefab, transform.position, Quaternion.identity).GetComponent<RollingBoulderController>();
            boulderController.Initiate(Mathf.Sign(PlayerController._instance.playerTransform.position.x - transform.position.x));

            entityController.animator.SetTrigger(anim_Attack);

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

            behaviourStateMachine.SetState(_RandomStrollState);
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

    public override void Inform(InformEnum stateInfo)
    {
        switch (stateInfo)
        {            
            case InformEnum.OutOfRangeTimer:
                behaviourStateMachine.SetState(_RandomStrollState);
                aggressionStateMachine.SetState(_NullState);
                targetingPlayer = false;
                entityController.animator.SetBool(anim_IsTargeting, false);
                break;

            default:
                InvalidInform.Error(stateInfo, this);
                break;

        }
    }
}
