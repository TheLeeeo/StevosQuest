using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombHeadAI : AIBase
{
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, range);
    }

    #region AnimatorHashes
    public static int anim_JumpHash;
    public static int anim_ExplodeHash;
    public static int anim_MovementSpeed;
    public static int anim_IsGrounded;

    static BombHeadAI()
    {
        anim_JumpHash = Animator.StringToHash("Jump");
        anim_ExplodeHash = Animator.StringToHash("Explode");
        anim_MovementSpeed = Animator.StringToHash("IsMoving");
        anim_IsGrounded = Animator.StringToHash("IsGrounded");
    }
    #endregion

    #region StateSetup
    private RandomStrollToEdge _RandomStrollState;
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
    private GameObject explosionObject;

    [SerializeField]
    private float explosionTime;

    [SerializeField]
    private float range;
    [SerializeField]
    private float knockbackForce;

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

            behaviourStateMachine.SetState(_RandomStrollState);
        }
    }

    public override void AttackPlayer()
    {
        entityController.animator.SetTrigger(anim_ExplodeHash);

        this.enabled = false;

        StartCoroutine(Explode());
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(explosionTime);

        SoundEffectController controller = Instantiate(PrefabManager._instance.SoundEffectPrefab, transform.position.normalized, Quaternion.identity).GetComponent<SoundEffectController>();
        controller.PlaySoundEffect(entityController.soundController.attackSound);

        Instantiate(explosionObject, entityController.transform.position, entityController.transform.rotation);

        Vector2 direction = PlayerController._instance.playerTransform.position - transform.position;

        if (direction.magnitude <= range)
        {
            PlayerHealth._instance.Damage((int)(baseDamage * LevelData.GetEntityPowerMultiplier()));            

            PlayerMovement._instance.isAffectedByKnockback = true;
            PlayerController._instance.entityRigidbody.velocity = GetMagnitude(direction.magnitude) * direction.normalized;
        }

        entityController.deathEvent.Invoke();

        Destroy(gameObject);
    }

    private float GetMagnitude(float distance)
    {
        return knockbackForce / -(range * range) * (distance * distance - range * range);
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

    public void Jump()
    {
        entityController.animator.SetTrigger(anim_JumpHash);
    }

    private void Start()
    {
        _RandomStrollState = new RandomStrollToEdge(this, entityController) { MinWaitTime = this.MinWaitTime, MaxWaitTime = this.MaxWaitTime, MinMoveDist = this.MinMoveDist, MaxMoveDist = this.MaxMoveDist };
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
