using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveIdleState : State
{
    public WaveIdleState(IStateInform _informTarget_, EntityController _entityController)
    {
        informTarget = _informTarget_;
        entityController = _entityController;
    }

    public float TimeToWait = 0;

    private float currentTimer;
    public float waveScale = 1;

    private float _fullWaveTimeMultiplier = Mathf.PI;
    public float timeForFullWave
    {
        get
        {
            return Mathf.PI / _fullWaveTimeMultiplier;
        }

        set
        {
            _fullWaveTimeMultiplier = Mathf.PI / value;
        }
    }

    public override void StateEnter()
    {
        currentTimer = 0;
    }

    public override void StateUpdate()
    {
        entityController.entityRigidbody.position += waveScale * Time.deltaTime * Mathf.Sin(2 * Time.time * _fullWaveTimeMultiplier) * _fullWaveTimeMultiplier * Vector2.up;

        currentTimer += Time.deltaTime;

        if (currentTimer >= TimeToWait)
        {
            currentTimer = 0;
            informTarget.Inform(InformEnum.TimerFinished);
        }
    }
}
