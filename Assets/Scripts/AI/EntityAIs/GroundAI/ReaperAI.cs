using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaperAI : AIBase
{
    #region AnimatorHashes
    public static int anim_AttackHash;

    static ReaperAI()
    {
        anim_AttackHash = Animator.StringToHash("Attack");
    }
    #endregion

    #region StateSetup
    private RandomStrollToEdge _RandomStrollState;
    #endregion


    [SerializeField]
    private float MinWaitTime;
    [SerializeField]
    private float MaxWaitTime;
    [SerializeField]
    private float MinMoveDist;
    [SerializeField]
    private float MaxMoveDist;

    [SerializeField]
    private int baseDamage;

    [SerializeField]
    private float attackTime;
    private float timeOfLastAttack = 0;

    public override void AttackPlayer()
    {
        if (Time.time - timeOfLastAttack >= attackTime)
        {
            entityController.soundController.PlayAttackSound();

            entityController.animator.SetTrigger(anim_AttackHash);
            PlayerHealth._instance.Damage((int)(baseDamage * LevelData.GetEntityPowerMultiplier()));
            timeOfLastAttack = Time.time;
        }
    }

    private void Start()
    {
        _RandomStrollState = new RandomStrollToEdge(this, entityController) { MinWaitTime = this.MinWaitTime, MaxWaitTime = this.MaxWaitTime, MinMoveDist = this.MinMoveDist, MaxMoveDist = this.MaxMoveDist };

        behaviourStateMachine.SetState(_RandomStrollState);
    }

    private void FixedUpdate()
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
