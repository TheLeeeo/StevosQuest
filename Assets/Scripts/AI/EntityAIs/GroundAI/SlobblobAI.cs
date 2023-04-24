using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlobblobAI : AIBase
{
    #region AnimatorHashes
    public static int anim_JumpHash;
    public static int anim_MovementSpeed;
    public static int anim_IsGrounded;

    static SlobblobAI()
    {
        anim_JumpHash = Animator.StringToHash("Jump");
        anim_MovementSpeed = Animator.StringToHash("IsMoving");
        anim_IsGrounded = Animator.StringToHash("IsGrounded");
    }
    #endregion

    private State_JumpToPlayer _JumpToPLayerAndAttackState;
    private State_RandomStroll _RandomStrollState;

    private State_Null _NullState = new State_Null();
    private State_TimeAfterPlayerLost _TimeAfterPlayerLostState;

    private StateMachine aggressionStateMachine = new StateMachine();

    [SerializeField]
    private float MinWaitTime;
    [SerializeField]
    private float MaxWaitTime;
    [SerializeField]
    private float MinMoveDist;
    [SerializeField]
    private float MaxMoveDist;

    [SerializeField]
    private float JumpDistance;
    [SerializeField]
    private float JumpHeight;

    [SerializeField]
    private int baseDamage;

    public override void AggrevateOnPlayer()
    {
        if (false == targetingPlayer)
        {
            base.AggrevateOnPlayer();

            behaviourStateMachine.SetState(_JumpToPLayerAndAttackState);
            targetingPlayer = true;
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

    public override void AttackPlayer()
    {
        PlayerHealth._instance.Damage((int)(baseDamage * LevelData.GetEntityPowerMultiplier()));
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
        _RandomStrollState = new State_RandomStroll(this, entityController) { MinMoveDist = this.MinMoveDist, MaxMoveDist = this.MaxMoveDist, MinWaitTime = this.MinWaitTime, MaxWaitTime = this.MaxWaitTime };
        _JumpToPLayerAndAttackState = new State_JumpToPlayer(this, entityController) { JumpDistance = 2 * this.JumpDistance, JumpHeight = this.JumpHeight };
        _TimeAfterPlayerLostState = new State_TimeAfterPlayerLost(this, 5);

        behaviourStateMachine.SetState(_RandomStrollState);
        aggressionStateMachine.SetState(_NullState);
    }

    private void FixedUpdate()
    {
        entityController.animator.SetBool(anim_MovementSpeed, entityController.isMoving);
        entityController.animator.SetBool(anim_IsGrounded, entityController.isGrounded);

        behaviourStateMachine.Update();
        aggressionStateMachine.Update();
    }

    public void Jump()
    {
        entityController.animator.SetTrigger(anim_JumpHash);
    }

    public override void Inform(InformEnum stateInfo)
    {
        switch (stateInfo)
        {
            case InformEnum.OutOfRangeTimer:
                behaviourStateMachine.SetState(_RandomStrollState);
                aggressionStateMachine.SetState(_NullState);
                targetingPlayer = false;
                break;

            default:
                InvalidInform.Error(stateInfo, this);
                break;

        }
    }
}
