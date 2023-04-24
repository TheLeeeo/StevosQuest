using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_RocketFlyToPlayer : State
{
    public State_RocketFlyToPlayer(IStateInform _informTarget, EntityController _entityController)
    {
        informTarget = _informTarget;
        entityController = _entityController;
    }

    public int MaxTurnAngle;

    public override void StateEnter()
    {
        entityController.isMoving = true;
    }

    public override void StateUpdate()
    {
        entityController.transform.rotation = Quaternion.RotateTowards(entityController.transform.rotation, Quaternion.FromToRotation(Vector3.up, PlayerController._instance.transform.position - entityController.transform.position), MaxTurnAngle * Time.deltaTime);

        entityController.entityRigidbody.velocity = entityController.movementSpeed * entityController.transform.up;
    }

    public override void StateExit()
    {
        entityController.isMoving = false;
    }
}
