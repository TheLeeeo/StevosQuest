using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionArrowController : MonoBehaviour
{
    public static DirectionArrowController _instance;

    private void Awake()
    {
        if (_instance == null) //Checks for other instances of class, if another instance can not be found, make this the instance
        {
            _instance = this;
        }
        else //If another instance exists, throw error with path to both instances and then destroy this script
        {
            Debug.LogError("Second instance of singleton class \"" + this + "\" created in:\n" + gameObject + "\n\n" + "Original instance is located in:\n" + _instance.gameObject + "\n");
            Destroy(this);
        }
    }

    [SerializeField] private float maxDistanceFromPlayer;

    public Transform target;
    private Transform playerPosition;

    public void Start()
    {
        playerPosition = PlayerController._instance.playerTransform;

        gameObject.SetActive(false);
    }

    public void Update()
    {
        Vector3 directionVector = target.position - playerPosition.position;
        float distance = directionVector.magnitude;

        directionVector /= distance;
        directionVector *= Mathf.Min(distance, maxDistanceFromPlayer);

        transform.position = playerPosition.position + directionVector;
        transform.up = directionVector;
    }
}
