using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_FlyFollowTransform : State
{
    public State_FlyFollowTransform(IStateInform _informTarget, EntityController _entityController)
    {
        informTarget = _informTarget;
        entityController = _entityController;

        _FlyState = new State_Fly(this, entityController);
    }

    private State_Fly _FlyState;

    private StateMachine stateMachine = new StateMachine();

    private Vector2 currentDirection;

    public Transform target;

    public float followDistance;

    public override void StateEnter()
    {
        _FlyState.distance = int.MaxValue;
        stateMachine.SetState(_FlyState);

        currentDirection.x = entityController.isFacingRight ? 1 : -1;
    }

    public override void StateUpdate()
    {
        Vector2 newDirection = (Vector2)target.position - entityController.entityRigidbody.position;
        float distanceToTarget = newDirection.magnitude;

        if (Mathf.Sign(newDirection.x) != Mathf.Sign(currentDirection.x))
        {
            entityController.Flip();
        }

        currentDirection = newDirection;


        newDirection /= distanceToTarget; //Normalized

        newDirection *= Mathf.Min(1, distanceToTarget - followDistance);

        _FlyState.SetNormalizedDirection(newDirection);

        stateMachine.Update();
    }    
}
