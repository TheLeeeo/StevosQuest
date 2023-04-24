using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{
    [SerializeField]
    private new Transform transform;

    [SerializeField]
    private Transform[] waypoints;
    private int currentWaypointIndex = 0;

    private const float SquareWaypointRadius = 0.02f;

    [SerializeField]
    private float speed;

    private void Update()
    {
        if ((waypoints[currentWaypointIndex].position - transform.position).sqrMagnitude < SquareWaypointRadius) //arrived to the waypoint
        {
            if (currentWaypointIndex == waypoints.Length - 1)
            {
                currentWaypointIndex = 0;
            }
            else
            {
                currentWaypointIndex += 1;
            }

        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].position, Time.deltaTime * speed);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((1 << collision.gameObject.layer & CommonLayerMasks.HasHealth) > 0)
        {
            collision.transform.SetParent(transform);
        }        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if ((1 << collision.gameObject.layer & CommonLayerMasks.HasHealth) > 0)
        {
            collision.transform.SetParent(null);
        }
    }
}
