using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropToNodeState : State
{
    public DropToNodeState(IStateInform _informTarget_, EntityController _entityController_)
    {
        informTarget = _informTarget_;
        entityController = _entityController_;
    }

    public void SetTarget(IntVector2 targetNode)
    {
        dropTarget = WorldGrid.GridToWorldPosition(targetNode).x + 0.5f;
        direction = Mathf.Sign(dropTarget - entityController.entityRigidbody.position.x);
    }

    private float dropTarget;

    private float direction;

    private bool hasLeftGround;

    public override void StateEnter()
    {
        entityController.isMoving = true;
        hasLeftGround = false;
    }

    public override void StateUpdate()
    {
        float speed = entityController.movementSpeed * direction;

        if (GroundCheck.CheckForGround(entityController.AI.groundCollider))
        {
            if (hasLeftGround)
            {
                entityController.isGrounded = true;
                entityController.entityRigidbody.velocity = Vector2.zero;
                informTarget.Inform(InformEnum.DropCompleted);

                return;
            }
        }
        else
        {
            entityController.isMoving = false;
            hasLeftGround = true;
            entityController.isGrounded = false;
            speed = entityController.movementSpeed * Mathf.Clamp(dropTarget - (entityController.AI.groundCollider.bounds.center.x -(entityController.AI.groundCollider.bounds.extents.x) * direction), -1, 1);
        }
        entityController.entityRigidbody.position += new Vector2(speed * Time.deltaTime, 0);
    }
}
