using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventOnCollision : MonoBehaviour
{
    public UnityEvent onCollisionEvent;

    private void OnTriggerEnter2D()
    {
        onCollisionEvent.Invoke();
    }

    private void OnCollisionEnter2D()
    {
        onCollisionEvent.Invoke();
    }
}
