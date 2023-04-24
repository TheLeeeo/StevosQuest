using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedBallAI : AIBase
{
    #region AnimatorHashes
    public static int anim_AttackHash;

    static RedBallAI()
    {
        anim_AttackHash = Animator.StringToHash("Attack");
    }
    #endregion

    [SerializeField]
    private float size;

    [SerializeField]
    private int baseDamage;

    public override void AggrevateOnPlayer()
    {
        if (false == targetingPlayer)
        {
            base.AggrevateOnPlayer();

            Spin((int)Mathf.Sign(PlayerController._instance.playerTransform.position.x - transform.position.x));

            entityController.animator.SetTrigger(anim_AttackHash);

            targetingPlayer = true;
        }
    }

    public override void NotifyOfAttack(int attackDirection)
    {
        AggrevateOnPlayer();
    }

    private void Spin(int direction)
    {
        entityController.entityRigidbody.velocity = new Vector2(entityController.movementSpeed * direction, 0);
        float rotationalSpeed = 180 * entityController.movementSpeed / (size * Mathf.PI);
        entityController.entityRigidbody.angularVelocity = rotationalSpeed * -direction;
    }

    public override void AttackPlayer()
    {
        PlayerHealth._instance.Damage((int)(baseDamage * LevelData.GetEntityPowerMultiplier()));
    }

    public override void PlayerEnteredDetection()
    {
        AggrevateOnPlayer();
    }

    private void FixedUpdate()
    {
        if (targetingPlayer && Mathf.Abs(entityController.entityRigidbody.velocity.x) < 0.1f)
        {
            entityController.health.Die();
        }
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
