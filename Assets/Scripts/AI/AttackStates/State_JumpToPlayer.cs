using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_JumpToPlayer : State
{
    public State_JumpToPlayer(IStateInform _informTarget, EntityController _entityController)
    {
        informTarget = _informTarget;
        entityController = _entityController;

        _JumpXState = new JumpXState(this, entityController);
    }

    private JumpXState _JumpXState;

    private StateMachine stateMachine = new StateMachine();

    public float JumpDistance;
    public float JumpHeight;

    private int currentDirection;

    public override void StateEnter()
    {
        _JumpXState.jumpDistance = this.JumpDistance;
        _JumpXState.jumpHeight = this.JumpHeight;

        _JumpXState.distance = int.MaxValue;
        stateMachine.SetState(_JumpXState);

        currentDirection = entityController.isFacingRight ? 1 : -1;

        _JumpXState.SetDirection(currentDirection);
    }

    public override void StateExit()
    {
        entityController.entityRigidbody.velocity = Vector2.zero;
        entityController.isMoving = false;
    }

    public override void StateUpdate()
    {
        /*if (true == entityController.AI.PlayerInAttackRange)
        {
            entityController.AI.AttackPlayer();
        }
        else*/
        {
            if(true == entityController.isGrounded)
            {
                int newDirection = (int)Mathf.Sign(PlayerController._instance.entityRigidbody.position.x - entityController.entityRigidbody.position.x);
                if (newDirection != currentDirection)
                {
                    entityController.Flip();

                    currentDirection = newDirection;

                    _JumpXState.SetDirection(newDirection);
                }

                _JumpXState.jumpDistance = Mathf.Min(JumpDistance, Mathf.Abs(PlayerController._instance.entityRigidbody.position.x - entityController.entityRigidbody.position.x));
            }

            stateMachine.Update();
        }
    }

    public override void Inform(InformEnum stateInfo)
    {
        switch (stateInfo)
        {
            case InformEnum.MovementCompleted:
            case InformEnum.ReachedWall:
                break;

            default:
                InvalidInform.Error(stateInfo, this);
                break;
        }
    }
}
