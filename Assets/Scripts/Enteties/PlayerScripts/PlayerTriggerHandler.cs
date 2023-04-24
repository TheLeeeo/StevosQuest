using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTriggerHandler : MonoBehaviour
{
    public static PlayerTriggerHandler _instance;

    private void Awake()
    {
        if (_instance == null) //Checks for other instances of class, if another instance can not be found, make this the instance
        {
            _instance = this;
        }
        else //If another instance exists, throw error with path to both instances and then destroy this script
        {
            Debug.LogError("Instance of singleton class \"" + this + "\" already exists in:\n" + gameObject + "\n\n" + "Original instance is located in:\n" + _instance.gameObject + "\n");
            Destroy(this);
        }
    }

    public Vector2 size;

    [SerializeField] private LayerMask interactableLayer;

    private IIsInteractable currentItem;
    private Collider2D currentItemCollider;

    private void FixedUpdate()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, size, 0, interactableLayer);

        if (0 != colliders.Length)
        {
            float minDist = size.sqrMagnitude;
            int minIndex = 0;

            for(int i = 0; i < colliders.Length; i++)
            {
                float dist = (colliders[i].transform.position - PlayerController._instance.playerTransform.position).sqrMagnitude;

                if(dist < minDist)
                {
                    minDist = dist;
                    minIndex = i;
                }
            }

            if (currentItemCollider != colliders[minIndex]) //new item
            {
                currentItemCollider = colliders[minIndex];
                currentItem = currentItemCollider.GetComponent<IIsInteractable>();
                currentItem.PlayerInRange();
            }
        }
        else
        {
            currentItem = null;
            currentItemCollider = null;
            InteractTextController._instance.DisableText();
        }

        //-----------------------------//Old code//
        /*currentItemCollider = Physics2D.OverlapBox(transform.position, size, 0, itemLayer);

        currentlyInteracting = currentItemCollider != null;

        if (currentlyInteracting)
        {
            InteractTextController._instance.SetupText(currentItemCollider.transform.position, "Pickup");
        } else
        {
            InteractTextController._instance.DisableText();
        }*/
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            currentItem?.PlayerInteraction();
        }
    }
}
