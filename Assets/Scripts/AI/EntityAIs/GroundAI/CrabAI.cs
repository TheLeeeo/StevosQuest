using UnityEngine;

public class CrabAI : AIBase
{
    #region AnimatorHashes
    public static int anim_JumpHash;
    public static int anim_AttackHash;
    public static int anim_MovementSpeed;
    public static int anim_IsGrounded;

    static CrabAI()
    {
        anim_JumpHash = Animator.StringToHash("Jump");
        anim_AttackHash = Animator.StringToHash("Attack");
        anim_MovementSpeed = Animator.StringToHash("IsMoving");
        anim_IsGrounded = Animator.StringToHash("IsGrounded");
    }
    #endregion

    #region StateSetup
    private State_RandomStroll _RandomStrollState;
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
        if(false == targetingPlayer)
        {
            if ((entityController.isFacingRight && -1 == attackDirection) || (!entityController.isFacingRight && 1 == attackDirection)) //looking in the wrong direction
            {
                entityController.Flip();
            }

            behaviourStateMachine.SetState(_RandomStrollState);
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
            entityController.soundController.PlayAttackSound();

            entityController.animator.SetTrigger(anim_AttackHash);
            PlayerHealth._instance.Damage((int)(baseDamage * LevelData.GetEntityPowerMultiplier()));
            timeOfLastAttack = Time.time;
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

    private void Start()
    {
        _RandomStrollState = new State_RandomStroll(this, entityController) { MinWaitTime = this.MinWaitTime, MaxWaitTime = this.MaxWaitTime, MinMoveDist = this.MinMoveDist, MaxMoveDist = this.MaxMoveDist };
        _MoveToPlayerAndAttackState = new State_RunToPlayerAndAttack(this, entityController);
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

    public override void Inform(InformEnum stateInfo)
    {
        switch (stateInfo)
        {            
            /*case InformEnum.TimerFinished:
                behaviourStateMachine.SetState(_RandomStrollState);
                break;*/

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
