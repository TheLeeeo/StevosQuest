using System.Timers;
using UnityEngine;

public sealed class RandomTimerState : State
{
    public RandomTimerState(IStateInform _informTarget_, float _MinTime_, float _MaxTime_)
    {
        MinTime = _MinTime_;
        MaxTime = _MaxTime_;

        informTarget = _informTarget_;
    }

    public float MinTime;
    public float MaxTime;

    private float TimeToWait;

    public override void StateEnter()
    {
        TimeToWait = Random.Range(MinTime, MaxTime);
    }

    public override void StateUpdate()
    {
        TimeToWait -= Time.deltaTime;

        if (TimeToWait >= 0)
        {
            StateEnter();
            informTarget.Inform(InformEnum.TimerFinished);
        }
    }

    

}
