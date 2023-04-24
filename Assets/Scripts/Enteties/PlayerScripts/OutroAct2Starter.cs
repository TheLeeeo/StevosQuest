using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class OutroAct2Starter : MonoBehaviour
{
    public UnityEvent onStartEvent;

    private bool canBeStarted = false;
    private bool alreadyBegun = false;

    public void CanBeStarted()
    {
        canBeStarted = true;
    }

    public void StartScene2(InputAction.CallbackContext context)
    {
        if (context.started && canBeStarted && !alreadyBegun)
        {
            alreadyBegun = true;
            onStartEvent.Invoke();
        }
    }
}
