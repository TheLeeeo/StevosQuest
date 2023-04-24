using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmilingRocketAI : AIBase
{
    public static int anim_Smile;

    static SmilingRocketAI()
    {
        anim_Smile = Animator.StringToHash("Smile");
    }    

    private State_RocketFlyToPlayer _RocketFlyState; //Attack

    private State_Null _NullState = new State_Null();

    [SerializeField]
    private int MaxTurnAngle;

    [SerializeField]
    private GameObject ExplosionEffectObject;

    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private float range;

    [SerializeField]
    private float knockbackForce;

    [SerializeField]
    private int baseDamage;

    private void Start()
    {
        _RocketFlyState = new State_RocketFlyToPlayer(this, entityController)
        {
            MaxTurnAngle = this.MaxTurnAngle
        };

        //aggressionStateMachine.SetState(_NullState);
        behaviourStateMachine.SetState(_NullState);
    }

    public override void AttackPlayer()
    {
        Explode();
    }

    public override void NotifyOfAttack(int attackDirection)
    {
        if (false == targetingPlayer)
        {
            if ((entityController.isFacingRight && -1 == attackDirection) || (!entityController.isFacingRight && 1 == attackDirection)) //looking in the wrong direction
            {
                entityController.Flip();
            }

            AggrevateOnPlayer();
        }
    }

    public override void AggrevateOnPlayer()
    {
        if (false == targetingPlayer)
        {
            base.AggrevateOnPlayer();

            behaviourStateMachine.SetState(_RocketFlyState);
            targetingPlayer = true;

            entityController.soundController.PlaySecondarySound();

            entityController.animator.SetTrigger(anim_Smile);
        }
    }

    public override void PlayerEnteredDetection()
    {
        AggrevateOnPlayer();
    }

    public void FixedUpdate()
    {
        behaviourStateMachine.Update();
        //aggressionStateMachine.Update();
    }

    private void OnCollisionEnter2D()
    {
        if (behaviourStateMachine.CurrentState == _RocketFlyState)
        {
            Explode();
        }
    }

    private void Explode()
    {
        Instantiate(ExplosionEffectObject, transform.position, Quaternion.identity);

        spriteRenderer.enabled = false;

        SoundEffectController controller = Instantiate(PrefabManager._instance.SoundEffectPrefab, transform.position.normalized, Quaternion.identity).GetComponent<SoundEffectController>();
        controller.PlaySoundEffect(entityController.soundController.attackSound);

        if ((PlayerController._instance.playerTransform.position - entityController.transform.position).magnitude < range)
        {
            PlayerController._instance.health.Damage(Mathf.CeilToInt(baseDamage * LevelData.GetEntityPowerMultiplier()));

            Vector2 direction = PlayerController._instance.playerTransform.position - transform.position;

            PlayerMovement._instance.isAffectedByKnockback = true;
            PlayerController._instance.entityRigidbody.velocity = GetMagnitude(direction.magnitude) * direction.normalized;
        }

        this.enabled = false;
        Destroy(gameObject);
    }

    private float GetMagnitude(float distance)
    {
        return knockbackForce / -(range * range) * (distance * distance - range * range);
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
