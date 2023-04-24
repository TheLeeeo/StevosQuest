using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventOnTriggerEnter : MonoBehaviour
{
    public UnityEvent triggerEvent;

    private void OnTriggerEnter2D()
    {
        triggerEvent.Invoke();
    }
}
