using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InvokeEntityEventOnTriggerEnter : MonoBehaviour
{
    public UnityEvent<EntityController> OnTriggerEnterEvent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnTriggerEnterEvent.Invoke(collision.GetComponent<EntityController>());
    }
}
