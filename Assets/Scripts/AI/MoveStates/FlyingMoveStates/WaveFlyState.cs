using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveFlyState : State //does not check for collisions
{
    public WaveFlyState(IStateInform _informTarget, EntityController _entityController)
    {
        informTarget = _informTarget;
        entityController = _entityController;
    }

    private Vector2 direction;
    private Vector2 waveVector;

    public float distance;
    public float distanceRemaining;

    private float _fullWaveTimeMultiplier = Mathf.PI;
    public float timeForFullWave
    {
        get
        {
            return  Mathf.PI / _fullWaveTimeMultiplier;
        }

        set
        {
            _fullWaveTimeMultiplier = Mathf.PI / value;
        }
    }
    public float waveScale = 1;

    public void SetNormalizedDirection(Vector2 newDirection)
    {
        direction = newDirection;
        waveVector = new Vector2(-direction.y, direction.x);      
    }

    /// <summary>
    /// Normalizes the direction before asigning
    /// </summary>
    public void SetNotNormalDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
        waveVector = new Vector2(-direction.y, direction.x);
    }

    public override void StateEnter()
    {
        entityController.isMoving = true;
        distanceRemaining = distance;
    }

    public override void StateUpdate()
    {
        entityController.entityRigidbody.velocity = entityController.movementSpeed * direction;

        entityController.entityRigidbody.position += waveScale * Time.deltaTime * Mathf.Sin(2 * Time.time * _fullWaveTimeMultiplier) * _fullWaveTimeMultiplier * waveVector;

        distanceRemaining -= Time.deltaTime * entityController.movementSpeed;

        if (distanceRemaining <= 0)
        {
            informTarget.Inform(InformEnum.Flight_MovementCompleted);
        }
    }

    public override void StateExit()
    {
        entityController.isMoving = false;
    }
}
