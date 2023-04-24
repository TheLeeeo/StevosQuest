using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageTextController : MonoBehaviour
{
    private static AnimationCurve ScaleCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0), new Keyframe(TotalTime / 3, 1) });
    private static AnimationCurve AlphaCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 1), new Keyframe(TotalTime * 2f / 3, 1), new Keyframe(TotalTime, 0) });

    private static readonly Vector3 BaseScale = new Vector3(0.5f, 0.5f, 1);

    [SerializeField] private Text damageText;

    [SerializeField] private Transform textTransform;

    private const float TotalTime = 2f;

    [SerializeField]
    private float timeAlive = 0;

    public void Setup(int value)
    {
        damageText.text = value.ToString();
    }

    private void Update()
    {
        timeAlive += Time.deltaTime;

        damageText.color = new Color(1, 1 - timeAlive / TotalTime, 0, AlphaCurve.Evaluate(timeAlive));

        textTransform.localScale = BaseScale * ScaleCurve.Evaluate(timeAlive);
        textTransform.position += new Vector3(0, Time.deltaTime / 3, 0);

        if(timeAlive > TotalTime)
        {
            Destroy(gameObject);
        }
    }
}
