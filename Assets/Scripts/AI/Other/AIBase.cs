using UnityEngine;

public abstract class AIBase : MonoBehaviour, IStateInform
{
    public EntityController entityController;    

    public Collider2D groundCollider;

    protected StateMachine behaviourStateMachine = new StateMachine();

    //[HideInInspector]
    public bool PlayerInAttackRange = false;

    [SerializeField]
    protected bool targetingPlayer = false;

    public abstract void Inform(InformEnum stateInfo);

    public virtual void AggrevateOnPlayer()
    {
        Instantiate(PrefabManager._instance.AggrevatedParticleEffect, transform); //possible pooling
    }
    public virtual void NotifyOfAttack(int attackDirection) { }
    public virtual void PlayerEnteredDetection() { }
    public virtual void PlayerLeftDetection() { } //Player no longer in detection zone
    public virtual void AttackPlayer() { }
}