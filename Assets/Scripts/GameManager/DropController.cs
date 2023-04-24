using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropController : MonoBehaviour
{
    public static DropController _instance;

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

    [SerializeField] private GameObject interactableItemPrefab;
    [SerializeField] private GameObject pickupItemPrefab;

    /// <summary>
    /// Drops and item with some velocity next to the entity
    /// </summary>
    public static void DropItem(Item itemToDrop, Vector3 worldPosition)
    {
        GameObject droppedItem = Instantiate(_instance.interactableItemPrefab, worldPosition, Quaternion.identity);
        float angle = Random.Range(-45, 45) * Mathf.Deg2Rad;
        droppedItem.GetComponent<Rigidbody2D>().velocity = new Vector2(4 * Mathf.Sin(angle), 4 * Mathf.Cos(angle));

        droppedItem.GetComponent<DroppedItemBase>().InstantiateDrop(itemToDrop);
    }

    /// <summary>
    /// Releases an item straight down on the ground
    /// </summary>
    public static void ReleaseItem(Item itemToRelease, Vector3 worldPosition)
    {
        Instantiate(_instance.interactableItemPrefab, worldPosition, Quaternion.identity).GetComponent<DroppedItemBase>().InstantiateDrop(itemToRelease);
    }

    public static void DropPickup(Item pickupToDrop, Vector3 worldPosition)
    {
        GameObject droppedItem = Instantiate(_instance.pickupItemPrefab, worldPosition, Quaternion.identity);
        float angle = Random.Range(-45, 45) * Mathf.Deg2Rad;
        droppedItem.GetComponent<Rigidbody2D>().velocity = new Vector2(4 * Mathf.Sin(angle), 4 * Mathf.Cos(angle));

        droppedItem.GetComponent<DroppedItemBase>().InstantiateDrop(pickupToDrop);
    }

    public static void ReleasePickup(Item pickupToRelease, Vector3 worldPosition)
    {
        Instantiate(_instance.pickupItemPrefab, worldPosition, Quaternion.identity).GetComponent<DroppedItemBase>().InstantiateDrop(pickupToRelease);
    }
}
