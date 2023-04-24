using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAttackArea : MonoBehaviour
{
    [SerializeField]
    private EntityController entityController;

    private void OnTriggerEnter2D()
    {
        entityController.AI.AttackPlayer();
    }

    private void OnTriggerExit2D()
    {
        entityController.AI.AttackPlayer();
    }
}
