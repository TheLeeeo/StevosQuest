using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToWallState : State
{
    public MoveToWallState(IStateInform _informTarget_, EntityController _entityController_)
    {
        informTarget = _informTarget_;
        entityController = _entityController_;
    }

    private int direction;
    public float distanceLeft;

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

    public override void StateEnter()
    {
        entityController.isMoving = true;
    }

    public override void StateUpdate()
    {
        if (distanceLeft <= 0)
        {
            informTarget.Inform(InformEnum.MovementCompleted);
        }
        else if (GroundCheck.CheckForWall(entityController.AI.groundCollider, direction)) //Has reached a wall
        {
            informTarget.Inform(InformEnum.ReachedWall);
        }
        else
        {
            entityController.entityRigidbody.position += entityController.movementSpeed * Time.deltaTime * direction * Vector2.right;
            distanceLeft -= entityController.movementSpeed * Time.deltaTime;
        }
    }

    public override void StateExit()
    {
        entityController.isMoving = false;
    }

}
