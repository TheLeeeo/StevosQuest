using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class OutroImageFader : MonoBehaviour
{
    [SerializeField] private Image image;

    public UnityEvent onCompleteEvent;

    public void Begin()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        Color color = image.color;

        while (image.color.a < 1)
        {
            color.a += Time.deltaTime;
            image.color = color;

            yield return null;
        }

        onCompleteEvent.Invoke();
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        Color color = image.color;

        while (image.color.a > 0)
        {
            color.a -= Time.deltaTime;
            image.color = color;

            yield return null;
        }        
    }
}
