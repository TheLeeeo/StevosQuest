using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Fly : State
{
    public State_Fly(IStateInform _informTarget, EntityController _entityController)
    {
        informTarget = _informTarget;
        entityController = _entityController;
    }

    private Vector2 direction;

    public float distance;
    public float distanceRemaining;


    public void SetNormalizedDirection(Vector2 newDirection)
    {
        direction = newDirection;
    }

    /// <summary>
    /// Normalizes the direction before asigning
    /// </summary>
    public void SetNotNormalDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
    }

    public override void StateEnter()
    {
        entityController.isMoving = true;
        distanceRemaining = distance;
    }

    public override void StateUpdate()
    {
        entityController.entityRigidbody.velocity = entityController.movementSpeed * direction;

        distanceRemaining -= Time.deltaTime * entityController.movementSpeed;

        if (distanceRemaining <= 0)
        {
            informTarget.Inform(InformEnum.Flight_MovementCompleted);
        }
    }

    public override void StateExit()
    {
        entityController.isMoving = false;
    }
}
