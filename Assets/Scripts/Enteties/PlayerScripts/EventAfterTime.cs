using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventAfterTime : MonoBehaviour
{
    public UnityEvent onTimerEvent;

    [SerializeField] private float time;
    [SerializeField] private int repeats = 1;
    private int currentRepeats = 0;

    public void StartTimer()
    {
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(time);
        currentRepeats++;
        onTimerEvent.Invoke();

        if (currentRepeats < repeats)
        {
            StartCoroutine(Timer());
        }
    }
}
