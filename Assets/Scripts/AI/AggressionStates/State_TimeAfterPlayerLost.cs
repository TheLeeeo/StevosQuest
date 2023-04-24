using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_TimeAfterPlayerLost : State
{
    public State_TimeAfterPlayerLost(IStateInform _informTarget, float timeToWait)
    {
        informTarget = _informTarget;

        waitTime = timeToWait;
    }

    public float waitTime;

    private float timer;

    public override void StateEnter()
    {
        timer = waitTime;
    }

    public override void StateUpdate()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            informTarget.Inform(InformEnum.OutOfRangeTimer);
            timer = waitTime;
        }
    }
}
