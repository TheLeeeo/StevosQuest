using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinMotionAndLookTowardsTransform : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private float magnitude = 1;

    private Vector3 startPosition;

    private float offset;

    private void Start()
    {
        offset = Random.Range(0, Mathf.PI);
        startPosition = transform.position;
    }

    private void Update()
    {
        transform.position = startPosition + new Vector3(0, magnitude / 2 * Mathf.Sin(Time.time + offset));

        if (transform.lossyScale.x * Mathf.Sign(target.position.x - transform.position.x) < 0)
        {
            CustomMath.FlipTransform(transform);
        }
    }
}
