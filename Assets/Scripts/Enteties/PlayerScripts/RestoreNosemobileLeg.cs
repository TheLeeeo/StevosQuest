using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestoreNosemobileLeg : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite sprite;
    [SerializeField] private Transform footTransform;
    [SerializeField] private Vector3 footPosition;

    public void Restore()
    {
        animator.enabled = false;
        spriteRenderer.sprite = sprite;

        footTransform.localPosition = footPosition;
    }
}
