using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToEdge : State
{
    public MoveToEdge(IStateInform _informTarget_, EntityController _entityController_)
    {
        informTarget = _informTarget_;
        entityController = _entityController_;
    }

    public void SetDirection(int _direction)
    {
        if (1 == _direction || -1 == _direction)
        {
            direction = _direction;
        }
        else
        {
            Debug.LogError("Direction has to be 1 or -1");
            direction = 0;
        }
    }

    private int direction = 0;
    public float distanceLeft;

    public override void StateUpdate()
    {
        if (distanceLeft <= 0)
        {
            entityController.isMoving = false;
            informTarget.Inform(InformEnum.MovementCompleted);
        }
        else if (GroundCheck.CheckForWall(entityController.AI.groundCollider, direction)) //Has reached a wall
        {
            entityController.isMoving = false;
            informTarget.Inform(InformEnum.ReachedWall);
        }
        else if (GroundCheck.CheckForHole(entityController.AI.groundCollider, direction)) //Has reached a hole
        {
            entityController.isMoving = false;
            informTarget.Inform(InformEnum.ReachedEdge);
        }
        else
        {
            entityController.entityRigidbody.position += entityController.movementSpeed * Time.deltaTime * direction * Vector2.right;
            distanceLeft -= entityController.movementSpeed * Time.deltaTime;
            entityController.isMoving = true;
        }
    }
}
