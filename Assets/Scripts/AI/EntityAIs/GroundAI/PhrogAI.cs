using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhrogAI : AIBase
{
    private State_JumpToPlayer _JumpToPLayerAndAttackState;
    private State_RandomJumpStrollingWithPause _JumpStrollState;

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
    private float JumpPauseTime;

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

            behaviourStateMachine.SetState(_JumpStrollState);
        }
    }

    public override void AttackPlayer()
    {
        entityController.soundController.PlayAttackSound();

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
        _JumpStrollState = new State_RandomJumpStrollingWithPause(this, entityController) { MinMoveDist = this.MinMoveDist, MaxMoveDist = this.MaxMoveDist, MinWaitTime = this.MinWaitTime, MaxWaitTime = this.MaxWaitTime, JumpDistance = this.JumpDistance, JumpHeight = this.JumpHeight, jumpWaitTime = this.JumpPauseTime };
        _JumpToPLayerAndAttackState = new State_JumpToPlayer(this, entityController) { JumpDistance = 2 * this.JumpDistance, JumpHeight = this.JumpHeight};
        _TimeAfterPlayerLostState = new State_TimeAfterPlayerLost(this, 5);

        behaviourStateMachine.SetState(_JumpStrollState);
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
                behaviourStateMachine.SetState(_JumpStrollState);
                aggressionStateMachine.SetState(_NullState);
                targetingPlayer = false;
                break;

            default:
                InvalidInform.Error(stateInfo, this);
                break;

        }
    }
}
