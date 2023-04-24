using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DisplayTextOnPointerHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private float AnimationSpeed = 0.3f;

    [SerializeField]
    private Vector3 activePosition;
    [SerializeField]
    private Vector3 unactivePosition;

    public RectTransform displayObjectTransform;

    [SerializeField]
    private Text displayText;

    private bool animationIsCompleted;

    public void SetText(string newText)
    {
        displayText.text = newText;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(false == animationIsCompleted)
        {
            LeanTween.cancel(displayObjectTransform);
        }

        animationIsCompleted = false;

        displayObjectTransform.gameObject.SetActive(true);

        LeanTween.move(displayObjectTransform, activePosition, AnimationSpeed);
        LeanTween.scale(displayObjectTransform, Vector3.one, AnimationSpeed).setOnComplete(_ => animationIsCompleted = true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (false == animationIsCompleted)
        {
            LeanTween.cancel(displayObjectTransform);
        }

        animationIsCompleted = false;

        LeanTween.move(displayObjectTransform, unactivePosition, AnimationSpeed).setOnComplete(_ => displayObjectTransform.gameObject.SetActive(false));
        LeanTween.scale(displayObjectTransform, Vector3.zero, AnimationSpeed).setOnComplete(_ => animationIsCompleted = true);
    }
}
