using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectDisplayController : MonoBehaviour
{
    [SerializeField]
    private Transform DisplayTransform;

    [SerializeField]
    private Image EffectSprite;

    [SerializeField]
    private GameObject IsNegativeBox;

    [SerializeField]
    private Text TimeText;

    [SerializeField]
    private DisplayTextOnPointerHover effectInfoText;

    private float time;

    [HideInInspector]
    public int currentIndex;

    public void UpdatePosition(int newIndex)
    {
        DisplayTransform.localPosition = new Vector3(120 * (newIndex + 1), -68.75f, 0);

        currentIndex = newIndex;
    }

    public void Initiate(Effect effect)
    {
        EffectSprite.sprite = effect.effectData.EffectSprite;
        EffectSprite.SetNativeSize();

        IsNegativeBox.SetActive(effect.effectData.IsNegative);

        if(effect.Duration != float.PositiveInfinity)
        {
            TimeText.gameObject.SetActive(true);

            TimeText.text = ((int)effect.Duration + 1).ToString();

            time = effect.Duration;
        }

        effectInfoText.SetText(effect.effectData.ToolTip);
        effectInfoText.displayObjectTransform.gameObject.SetActive(false);
    }

    private void Update()
    {
        time -= Time.deltaTime;
        TimeText.text = ((int)time + 1).ToString();
    }

    public void UpdateTime(float newTime)
    {
        TimeText.text = ((int)newTime + 1).ToString();

        time = newTime;
    }
}
