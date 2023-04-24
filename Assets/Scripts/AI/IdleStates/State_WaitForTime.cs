using UnityEngine;

public class State_WaitForTime : State
{
    public State_WaitForTime(IStateInform _informTarget_)
    {
        informTarget = _informTarget_;
    }

    public float TimeToWait = 0;

    private float currentTimer;

    public override void StateEnter()
    {
        currentTimer = 0;
    }

    public override void StateUpdate()
    {
        currentTimer += Time.deltaTime;

        if(currentTimer >= TimeToWait)
        {
            currentTimer = 0;
            informTarget.Inform(InformEnum.TimerFinished);
        }
    }
}