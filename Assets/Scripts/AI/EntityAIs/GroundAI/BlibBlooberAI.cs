using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlibBlooberAI : AIBase
{
    #region AnimatorHashes
    public static int anim_JumpHash;
    public static int anim_AttackHash;
    public static int anim_MovementSpeed;
    public static int anim_IsGrounded;

    static BlibBlooberAI()
    {
        anim_JumpHash = Animator.StringToHash("Jump");
        anim_AttackHash = Animator.StringToHash("Attack");
        anim_MovementSpeed = Animator.StringToHash("IsMoving");
        anim_IsGrounded = Animator.StringToHash("IsGrounded");
    }
    #endregion

    #region StateSetup
    private RandomStrollToEdge _StrollToEdgeState;
    private State_RunToPlayerAndAttack _MoveToPlayerAndAttackState;

    private State_Null _NullState = new State_Null();
    private State_TimeAfterPlayerLost _TimeAfterPlayerLostState;
    #endregion

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
    private float maxShootingDistance;

    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField]
    private Transform firePoint;

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

            behaviourStateMachine.SetState(_MoveToPlayerAndAttackState);
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

            behaviourStateMachine.SetState(_StrollToEdgeState);
        }
    }

    public void Jump() //Called by unity event
    {
        entityController.animator.SetTrigger(anim_JumpHash);
    }

    public override void AttackPlayer()
    {
        if (Time.time - timeOfLastAttack >= attackTime)
        {
            entityController.animator.SetTrigger(anim_AttackHash);

            ShootSporeBall();

            entityController.soundController.PlayAttackSound();

            timeOfLastAttack = Time.time;
        }
    }

    private void ShootSporeBall()
    {
        GameObject go = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        JumpParameters jp = JumpParameters.HighestXJump(Mathf.Min(PlayerController._instance.playerTransform.position.x - transform.position.x, maxShootingDistance), 6);

        Vector2 velocity = new Vector2(jp.Speed, jp.JumpForce);

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
        _StrollToEdgeState = new RandomStrollToEdge(this, entityController) { MinWaitTime = this.MinWaitTime, MaxWaitTime = this.MaxWaitTime, MinMoveDist = this.MinMoveDist, MaxMoveDist = this.MaxMoveDist };
        _MoveToPlayerAndAttackState = new State_RunToPlayerAndAttack(this, entityController);
        _TimeAfterPlayerLostState = new State_TimeAfterPlayerLost(this, 10);

        behaviourStateMachine.SetState(_StrollToEdgeState);
        aggressionStateMachine.SetState(_NullState);
    }

    private void FixedUpdate()
    {
        entityController.animator.SetBool(anim_MovementSpeed, entityController.isMoving);
        entityController.animator.SetBool(anim_IsGrounded, entityController.isGrounded);

        behaviourStateMachine.Update();
        aggressionStateMachine.Update();
    }

    public override void Inform(InformEnum stateInfo)
    {
        switch (stateInfo)
        {
            /*case InformEnum.TimerFinished:
                behaviourStateMachine.SetState(_RandomStrollState);
                break;*/

            case InformEnum.OutOfRangeTimer:
                behaviourStateMachine.SetState(_StrollToEdgeState);
                aggressionStateMachine.SetState(_NullState);
                targetingPlayer = false;
                break;

            default:
                InvalidInform.Error(stateInfo, this);
                break;

        }
    }
}
