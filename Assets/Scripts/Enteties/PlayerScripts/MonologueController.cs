using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class MonologueController : MonoBehaviour
{
    [SerializeField] private Text textComponent;

    [SerializeField] public string[] phrases;

    private int index = 0;

    public UnityEvent onComplete;

    private bool completed = false;

    private void Start()
    {
        textComponent.text = phrases[index];
    }

    public void NextPhrase()
    {
        index++;

        if (index >= phrases.Length)
        {
            completed = true;

            gameObject.SetActive(false);

            onComplete.Invoke();
        }
        else
        {
            textComponent.text = phrases[index];
        }
    }

    public void NextPhrase(InputAction.CallbackContext context)
    {
        if(false == context.started || completed)
        {
            return;
        }

        NextPhrase();
    }
}
