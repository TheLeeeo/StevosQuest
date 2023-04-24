using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParralaxScroller : MonoBehaviour
{
    [SerializeField]
    private new Transform transform;

    [SerializeField]
    [Range(0,1)]
    private float ScrollAmmount; // 1 is following the player, 0 is completely static

    private Vector3 startPosition;

    private Vector3 playerStartPosition;

    private void Start()
    {
        startPosition = transform.position;

        playerStartPosition = PlayerController._instance.playerTransform.position;
    }

    private void Update()
    {
        transform.position = startPosition + (PlayerController._instance.playerTransform.position - playerStartPosition) * ScrollAmmount;
    }
}
