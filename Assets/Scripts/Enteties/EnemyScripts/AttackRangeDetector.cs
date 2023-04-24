using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangeDetector : MonoBehaviour
{
    [SerializeField]
    private EntityController entityController;

    private void OnTriggerEnter2D()
    {
        entityController.AI.PlayerInAttackRange = true;
    }

    private void OnTriggerExit2D()
    {
        entityController.AI.PlayerInAttackRange = false;
    }
}
