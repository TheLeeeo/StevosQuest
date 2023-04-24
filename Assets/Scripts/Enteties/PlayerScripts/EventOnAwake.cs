using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventOnAwake : MonoBehaviour
{
    public UnityEvent onAwakeEvent;

    private void Awake()
    {
        onAwakeEvent.Invoke();
    }
}
