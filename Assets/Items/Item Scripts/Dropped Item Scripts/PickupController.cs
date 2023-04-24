using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : DroppedItemBase
{
    [SerializeField] private Rigidbody2D dropRigidbody;

    private PickupCore activePickupcore;

    private static float _detectionRange = 3;
    public static float DetectionRange
    {
        set
        {
            _detectionRange = value;
            SquareDetectionRange = _detectionRange * _detectionRange;
        }
        get
        {
            return _detectionRange;
        }

    }

    public static float SquareDetectionRange { private set; get; } = 9;

    public override void InstantiateDrop(Item _item)
    {
        base.InstantiateDrop(_item);

        activePickupcore = Instantiate(_item.itemData.itemObject, transform).GetComponent<PickupCore>();
    }

    private void FixedUpdate()
    {
        Vector2 vectorToPlayer = PlayerController._instance.entityRigidbody.position - dropRigidbody.position;

        if (vectorToPlayer.sqrMagnitude <= SquareDetectionRange) //Player is within range
        {
            if (activePickupcore.ValidatePickup(PlayerController._instance))
            {
                if (vectorToPlayer.sqrMagnitude < 1f) //Can be picked up
                {
                    PlayerInventory._instance.AddPickup((Pickup)droppedItem);
                    Destroy(gameObject);
                }

                dropRigidbody.gravityScale = 0;
                dropRigidbody.velocity += 5 * Time.fixedDeltaTime * vectorToPlayer;
            }
            else
            {
                dropRigidbody.gravityScale = 1;
            }
        }
        else
        {
            dropRigidbody.gravityScale = 1;
        }
    }
}
