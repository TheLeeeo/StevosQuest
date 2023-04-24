using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunXState : State
{
    public RunXState(IStateInform _informTarget_, EntityController _entityController_)
    {
        informTarget = _informTarget_;
        entityController = _entityController_;

        State_MoveToWall = new MoveToWallState(this, entityController);
        State_JumpToNode = new JumpToNodeState(this, entityController);
        State_WaitForGrounded = new State_WaitForGrounded(this, entityController);
    }

    private MoveToWallState State_MoveToWall;
    private JumpToNodeState State_JumpToNode;
    private State_WaitForGrounded State_WaitForGrounded;

    private StateMachine stateMachine = new StateMachine();

    private int direction;
    public float distance;

    public void SetDirection(int _direction)
    {
        if (1 == _direction || -1 == _direction)
        {
            direction = _direction;
            State_MoveToWall.SetDirection(direction);
        }
        else
        {
            Debug.LogError("Direction has to be 1 or -1");
            direction = 0;
        }
    }

    public override void StateEnter()
    {
        State_MoveToWall.distanceLeft = distance;

        stateMachine.SetState(State_WaitForGrounded);
    }

    public override void StateUpdate()
    {
        stateMachine.Update();
    }


    private void CheckForNodeAbove()
    {
        Bounds colliderBounds = entityController.AI.groundCollider.bounds;
        IntVector2 nextNodeAdress = WorldGrid.WorldToGridPosition(new Vector2(colliderBounds.center.x + (colliderBounds.extents.x + 1) * direction, colliderBounds.center.y - colliderBounds.extents.y + 1.1f));
        if (WorldGrid.NodeAtAdress(nextNodeAdress).IsEdge)
        {
            State_JumpToNode.SetTarget(nextNodeAdress);
            stateMachine.SetState(State_JumpToNode);
        }
        else
        {
            informTarget.Inform(InformEnum.MovementCompleted);
        }
    }


    public override void Inform(InformEnum stateInfo)
    {
        switch (stateInfo)
        {           
            case InformEnum.GotGrounded:
                stateMachine.SetState(State_MoveToWall);
                break;

            case InformEnum.MovementCompleted:
                entityController.entityRigidbody.velocity = Vector2.zero;
                informTarget.Inform(InformEnum.MovementCompleted);
                break;

            case InformEnum.ReachedWall:
                CheckForNodeAbove();
                break;

            case InformEnum.JumpCompleted:
                stateMachine.SetState(State_MoveToWall);
                break;

            case InformEnum.DropCompleted:
                stateMachine.SetState(State_MoveToWall);
                break;

            default:
                InvalidInform.Error(stateInfo, this);
                break;
        }
    }
}
