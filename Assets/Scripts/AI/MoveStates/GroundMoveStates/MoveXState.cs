using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveXState : State
{
    public MoveXState(IStateInform _informTarget_, EntityController _entityController_)
    {
        informTarget = _informTarget_;
        entityController = _entityController_;

        State_MoveToEdge = new MoveToEdge(this, entityController);
        State_JumpToNode = new JumpToNodeState(this, entityController);
        State_DropToNode = new DropToNodeState(this, entityController);
        State_WaitForGrounded = new State_WaitForGrounded(this, entityController);
    }

    private int direction = 1;
    public float distance;

    private MoveToEdge State_MoveToEdge;
    private JumpToNodeState State_JumpToNode;
    private DropToNodeState State_DropToNode;
    private State_WaitForGrounded State_WaitForGrounded;

    private StateMachine stateMachine = new StateMachine();

    public void SetDirection(int _direction)
    {
        if (1 == _direction || -1 == _direction)
        {
            direction = _direction;
            State_MoveToEdge.SetDirection(direction);

        }
        else
        {
            Debug.LogError("Direction has to be 1 or -1");
            direction = 0;
        }
    }

    /*private void CheckForPlatform()
    {
        Bounds colliderBounds = entityController.AI.groundCollider.bounds;
        IntVector2 currentNodeAdress = WorldGrid.WorldToGridPosition(new Vector2(colliderBounds.center.x, colliderBounds.center.y - colliderBounds.extents.y));
        if (WorldGrid.NodeAtAdress(currentNodeAdress + new IntVector2(direction, 0)).IsWalkable) //is on platform
        {
            stateMachine.SetState(State_MoveToEdge);
        }
        else if (GroundCheck.CheckForWall(entityController.AI.groundCollider, direction))
        {
            CheckForNodeAbove();
        } else
        {
            CheckForNodeBelow();
        }
    }*/

    private void CheckForNodeAbove()
    {
        Bounds colliderBounds = entityController.AI.groundCollider.bounds;
        IntVector2 nextNodeAdress = WorldGrid.WorldToGridPosition(new Vector2(colliderBounds.center.x + (colliderBounds.extents.x + 1) * direction, colliderBounds.center.y + 1));
        if (WorldGrid.NodeAtAdress(nextNodeAdress).IsEdge)
        {
            State_JumpToNode.SetTarget(nextNodeAdress);
            stateMachine.SetState(State_JumpToNode);
        } else
        {
            informTarget.Inform(InformEnum.MovementCompleted);
        }
    }

    private void CheckForNodeBelow()
    {
        Bounds colliderBounds = entityController.AI.groundCollider.bounds;
        IntVector2 nextNodeAdress = WorldGrid.WorldToGridPosition(new Vector2(colliderBounds.center.x + (colliderBounds.extents.x - 0.25f + 1) * direction, colliderBounds.center.y - 1));

        if (WorldGrid.NodeAtAdress(nextNodeAdress).IsWalkable)
        {            
            State_DropToNode.SetTarget(nextNodeAdress);
            stateMachine.SetState(State_DropToNode);
        }
        else
        {
            informTarget.Inform(InformEnum.MovementCompleted);
        }
    }

    public override void StateEnter()
    {
        State_MoveToEdge.distanceLeft = distance;

        stateMachine.SetState(State_WaitForGrounded);
    }

    public override void StateUpdate()
    {
        stateMachine.Update();
    }

    public override void Inform(InformEnum stateInfo)
    {
        switch (stateInfo)
        {            
            case InformEnum.GotGrounded:
                stateMachine.SetState(State_MoveToEdge);
                break;

            case InformEnum.MovementCompleted:
                entityController.entityRigidbody.velocity = Vector2.zero;
                informTarget.Inform(InformEnum.MovementCompleted);
                break;

            case InformEnum.ReachedWall:
                CheckForNodeAbove();
                break;

            case InformEnum.ReachedEdge:
                CheckForNodeBelow();
                break;

            case InformEnum.JumpCompleted:
                stateMachine.SetState(State_MoveToEdge);
                //CheckForPlatform();
                break;

            case InformEnum.DropCompleted:
                stateMachine.SetState(State_MoveToEdge);
                //CheckForPlatform();
                break;

            default:
                InvalidInform.Error(stateInfo, this);
                break;
        }
    }
}