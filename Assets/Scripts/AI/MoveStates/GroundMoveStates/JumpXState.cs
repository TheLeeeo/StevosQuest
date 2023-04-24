using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpXState : State
{
    public JumpXState(IStateInform _informTarget_, EntityController _entityController_)
    {
        informTarget = _informTarget_;
        entityController = _entityController_;
    }

    public float jumpDistance;
    public float jumpHeight;

    private int direction = 1;
    public float distance;
    private float distanceLeft;

    private bool hasLeftGround;

    private bool alreadyCollidedWithWall;

    private JumpParameters jp;

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

    private bool CheckForWall()
    {
        return GroundCheck.CheckForWall(entityController.AI.groundCollider, direction);
    }

    public override void StateEnter()
    {
        distanceLeft = distance;

        alreadyCollidedWithWall = false;
    }

    private void Jump()
    {
        if (distanceLeft <= 0)
        {
            entityController.entityRigidbody.velocity = Vector2.zero;
            informTarget.Inform(InformEnum.MovementCompleted);
        }
        else
        {
            entityController.isGrounded = false;

            distanceLeft -= jumpDistance;

            jp = JumpParameters.HighestXJump(jumpDistance, jumpHeight);

            entityController.entityRigidbody.velocity = new Vector2(direction * jp.Speed, jp.JumpForce);

            entityController.jumpEvent.Invoke();
        }        
    }

    public override void StateUpdate()
    {
        if (false == entityController.isGrounded)
        {
            entityController.entityRigidbody.velocity = new Vector2(jp.Speed * direction, entityController.entityRigidbody.velocity.y);

            if (entityController.entityRigidbody.velocity.y < 0.1f)
            {
                if (GroundCheck.CheckForGround(entityController.AI.groundCollider))
                {
                    entityController.isGrounded = true;
                    entityController.entityRigidbody.velocity = Vector2.zero;
                }
            }
            /*else
            {
                hasLeftGround = !GroundCheck.CheckForGround(entityController.AI.groundCollider);
            }*/  
        }
        else //is grounded
        {
            if (CheckForWall())
            {
                if (alreadyCollidedWithWall) //cant get over wall
                {
                    informTarget.Inform(InformEnum.ReachedWall);
                }
                else //attempt to jump over wall
                {
                    alreadyCollidedWithWall = true;

                    Jump();
                }
            }
            else
            {
                alreadyCollidedWithWall = false;

                Jump();
            }
        }
    }
}
