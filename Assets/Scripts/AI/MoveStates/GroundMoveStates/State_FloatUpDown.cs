using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_FloatUpDown : State
{
    public State_FloatUpDown(IStateInform _informTarget_, EntityController _entityController_)
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

    private int direction = 1;
    public float distance;
    private float distanceLeft;

    public override void StateEnter()
    {
        distanceLeft = distance;

        entityController.isMoving = true;
    }

    public override void StateUpdate()
    {
        if (distanceLeft <= 0)
        {
            direction *= -1;
            distanceLeft = distance;
        }        
        else
        {
            entityController.entityRigidbody.position += entityController.movementSpeed * Time.deltaTime * direction * Vector2.up;
            distanceLeft -= entityController.movementSpeed * Time.deltaTime;
        }
    }

    public override void StateExit()
    {
        entityController.isMoving = false;
    }
}
