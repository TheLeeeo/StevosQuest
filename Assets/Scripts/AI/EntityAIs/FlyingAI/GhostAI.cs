using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAI : AIBase
{
    private State_Null _NullState = new State_Null();
    private State_TurnToPlayer _TurnToPlayerState;

    [SerializeField]
    private float attackTime;
    private float timeOfLastAttack = 0;

    [SerializeField]
    private int baseDamage;

    private void Start()
    {
        _TurnToPlayerState = new State_TurnToPlayer(entityController);

        behaviourStateMachine.SetState(_NullState);
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
        }
    }

    public override void PlayerEnteredDetection()
    {
        targetingPlayer = true;
        behaviourStateMachine.SetState(_TurnToPlayerState);
    }

    public override void PlayerLeftDetection()
    {
        behaviourStateMachine.SetState(_NullState);
        targetingPlayer = false;
    }

    public void FixedUpdate()
    {
        behaviourStateMachine.Update();
    }

    public override void Inform(InformEnum stateInfo)
    {
        switch (stateInfo)
        {
            default:
                InvalidInform.Error(stateInfo, this);
                break;

        }
    }
}
