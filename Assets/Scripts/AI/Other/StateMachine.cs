using UnityEngine;

public class StateMachine
{
    public State CurrentState { get; private set; }

    public void Update()
    {
        CurrentState.StateUpdate();
    }

    public void ReloadState()
    {
        CurrentState.StateExit();
        CurrentState.StateEnter();
    }

    public void SetState(State NewState)
    {
        if(CurrentState != null)
        {
            CurrentState.StateExit(); //Run StateExit of old state
        }

        NewState.StateEnter(); //Run StateEnter of new state

        CurrentState = NewState; //Set current state to new state
    }
}