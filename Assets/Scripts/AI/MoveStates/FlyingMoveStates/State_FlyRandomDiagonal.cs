using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_FlyRandomDiagonal : State
{
    public State_FlyRandomDiagonal(IStateInform _informTarget, EntityController _entityController)
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

    public float minMoveDist;
    public float maxMoveDist;

    private float currentCycleDistLeft;

    private Vector2 directionVector = Vector2.zero;

    private void SetNewDirection()
    {
        currentCycleDistLeft = Random.Range(minMoveDist, maxMoveDist);

        float angle = Random.Range(-_maxAngle, _maxAngle);
        int direction = RandomNumbers.RandomDirection();

        Vector2 newDirectionVector = new Vector2(Mathf.Cos(angle) * direction, Mathf.Sin(angle));

        if (Mathf.Sign(newDirectionVector.x) != Mathf.Sign(directionVector.x))
        {
            entityController.Flip();
        }

        directionVector = newDirectionVector;
    }

    public override void StateEnter()
    {
        SetNewDirection();

        directionVector.x = entityController.isFacingRight ? 1 : -1;
        entityController.isMoving = true;        
    }

    public override void StateUpdate()
    {
        entityController.entityRigidbody.velocity = entityController.movementSpeed * directionVector;

        currentCycleDistLeft -= Time.deltaTime * entityController.movementSpeed;

        if (currentCycleDistLeft <= 0)
        {
            SetNewDirection();
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
            entityController.isMoving = false;
        }
    }
}
