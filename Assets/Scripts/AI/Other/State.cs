using UnityEngine;

public abstract class State : IStateInform
{
    protected IStateInform informTarget;

    protected EntityController entityController;

    public virtual void StateEnter() { }

    public abstract void StateUpdate();

    public virtual void StateExit() { }

    public virtual void Inform(InformEnum stateInfo) { }
}