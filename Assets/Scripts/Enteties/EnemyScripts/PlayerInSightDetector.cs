using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInSightDetector : MonoBehaviour
{
    [SerializeField]
    private EntityController entityController;

    private void OnTriggerEnter2D()
    {
        //possibly a raycast to determine if player is in line of sight
        entityController.AI.PlayerEnteredDetection();
    }

    private void OnTriggerExit2D()
    {
        entityController.AI.PlayerLeftDetection();
    }
}
