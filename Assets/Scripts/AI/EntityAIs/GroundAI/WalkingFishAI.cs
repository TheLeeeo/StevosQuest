using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingFishAI : AIBase
{
    #region AnimatorHashes
    public static int anim_Jump;
    public static int anim_MovementSpeed;
    public static int anim_IsGrounded;

    static WalkingFishAI()
    {
        anim_Jump = Animator.StringToHash("Jump");
        anim_MovementSpeed = Animator.StringToHash("IsMoving");
        anim_IsGrounded = Animator.StringToHash("IsGrounded");
    }
    #endregion

    private State_RandomStroll _RandomStrollState;

    [SerializeField]
    private float MinWaitTime;
    [SerializeField]
    private float MaxWaitTime;
    [SerializeField]
    private float MinMoveDist;
    [SerializeField]
    private float MaxMoveDist;

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

    public void Jump()
    {
        entityController.animator.SetTrigger(anim_Jump);
    }

    private void Start()
    {
        _RandomStrollState = new State_RandomStroll(this, entityController) { MinWaitTime = this.MinWaitTime, MaxWaitTime = this.MaxWaitTime, MinMoveDist = this.MinMoveDist, MaxMoveDist = this.MaxMoveDist };

        behaviourStateMachine.SetState(_RandomStrollState);
    }

    private void FixedUpdate()
    {
        entityController.animator.SetBool(anim_MovementSpeed, entityController.isMoving);
        entityController.animator.SetBool(anim_IsGrounded, entityController.isGrounded);

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
