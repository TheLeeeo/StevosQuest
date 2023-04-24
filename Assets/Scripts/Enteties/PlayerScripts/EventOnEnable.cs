using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventOnEnable : MonoBehaviour
{
    public UnityEvent onEnableEvent;
    public UnityEvent onDisableEvent;

    private void OnEnable()
    {
        onEnableEvent.Invoke();
    }

    private void OnDisable()
    {
        onDisableEvent.Invoke();
    }
}
