using UnityEngine;

public class PatrolPlatform// : State
{
    /*public PatrolPlatform(EntityController _agentController_, float _timeToStandStill_)
    {
        AgentController = _agentController_;
        AgentAIBase = _agentController_.AI;

        WaitForTimeState = new WaitForTimeState(this, _timeToStandStill_);
        MoveState = new MoveXOnPlatform(AgentController, this, DirectionToMove);
    }

    private int DirectionToMove;

    StateMachine BehaviourStatemachine = new StateMachine();

    WaitForTimeState WaitForTimeState;
    MoveXOnPlatform MoveState;


    public override void StateEnter()
    {
        DirectionToMove = (AgentController.AI.ControlIterator % 2 == 0) ? -1 : 1; //-1 or 1
        MoveState.Direction = DirectionToMove;
        BehaviourStatemachine.SetState(MoveState);
    }

    public override void StateUpdate()
    {
        BehaviourStatemachine.CurrentState.StateUpdate();
    }

    public override void Inform(InformEnum PassedInfo)
    {
        switch (PassedInfo)
        {
            case InformEnum.TimerFinished:

                DirectionToMove *= -1;
                MoveState.Direction = DirectionToMove;

                BehaviourStatemachine.SetState(MoveState);
                break;

            case InformEnum.MovementCompleted:
                BehaviourStatemachine.SetState(WaitForTimeState);
                break;

            default:
                InvalidInform(PassedInfo);
                break;

        }
    }*/
}
