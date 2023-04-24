using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_DiagonalFlyToPlayerAndAttack : State
{
    public State_DiagonalFlyToPlayerAndAttack(IStateInform _informTarget, EntityController _entityController)
    {
        informTarget = _informTarget;
        entityController = _entityController;
    }

    public float maxAngle //Degrees
    {
        get => _maxAngle * Mathf.Rad2Deg;
        set => _maxAngle = value * Mathf.Deg2Rad;
    }
    private float _maxAngle; //Radians

    public float moveDist;

    private Vector2 directionVector = Vector2.zero;

    private float distanceLeft;

    private void SetNewDirection()
    {
        Vector2 vectorToPlayer = PlayerController._instance.entityRigidbody.position - entityController.entityRigidbody.position;
        float vectorMagnitude = vectorToPlayer.magnitude;

        float angleToPlayer = Mathf.Asin(vectorToPlayer.y / vectorMagnitude);

        angleToPlayer = Mathf.Clamp(angleToPlayer, -_maxAngle, _maxAngle);

        float direction = Mathf.Sign(vectorToPlayer.x);

        Vector2 newDirectionVector = new Vector2(Mathf.Cos(angleToPlayer) * direction, Mathf.Sin(angleToPlayer));

        if (Mathf.Sign(newDirectionVector.x) != Mathf.Sign(directionVector.x))
        {
            entityController.Flip();
        }

        directionVector = newDirectionVector;

        distanceLeft = moveDist;
    }

    public override void StateEnter()
    {
        directionVector.x = entityController.isFacingRight ? 1 : -1;

        SetNewDirection();

        entityController.isMoving = true;               
    }

    public override void StateUpdate()
    {
        if (true == entityController.AI.PlayerInAttackRange)
        {
            entityController.AI.AttackPlayer();
        }
        else if (distanceLeft >= 0)
        {
            distanceLeft -= Time.deltaTime;
        }
        else
        {
            SetNewDirection();

            entityController.entityRigidbody.velocity = entityController.movementSpeed * directionVector;            
        }       
    }

    public override void StateExit()
    {
        entityController.isMoving = false;
    }

    public void CollideWithGround(Vector2 collisionNormal)
    {
        if (Mathf.Abs(collisionNormal.x) >= 0.5f)
        {
            directionVector.x *= -1;
            entityController.Flip();
        }

        if (Mathf.Abs(collisionNormal.y) >= 0.5f)
        {
            directionVector.y *= -1;
        }

        entityController.entityRigidbody.velocity = entityController.movementSpeed * directionVector;
    }
}