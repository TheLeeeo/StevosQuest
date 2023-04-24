using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAtMouse : MonoBehaviour
{
    private bool isFacingRight;

    private Vector2 mousePositionInWorld;
    private float horizontalDistPlayerToMouse;

    private Camera mainCamera;

    private WeaponCore weaponCore;

    private Transform armPivot;

    private void Awake()
    {
        weaponCore = GetComponent<WeaponCore>();

        weaponCore.OnActivate += Activate;
        weaponCore.OnDeactivate += Deactivate;

        mainCamera = Camera.main;
    }

    private void Activate(EntityController _entityController)
    {
        PlayerMovement._instance.FlipCheckIsOverridden = true;

        isFacingRight = PlayerController._instance.isFacingRight;

        if (isFacingRight == false)
        {
            FlipSecondaryArmPivot();
        }

        armPivot = PlayerEquipedItemsController._instance.GetArmPivot();
    }

    private void Deactivate(EntityController _entityController)
    {
        PlayerMovement._instance.FlipCheckIsOverridden = false;
        
        if (isFacingRight == false)
        {
            FlipSecondaryArmPivot();

            PlayerEquipedItemsController._instance.RestoreFacing();
        }
    }


    private void FlipSecondaryArmPivot()
    {
        Vector3 temp = PlayerEquipedItemsController._instance.SecondArmPivot.localScale;
        temp.x *= -1;
        PlayerEquipedItemsController._instance.SecondArmPivot.localScale = temp;
    }
    
    private void LateUpdate()
    {
        mousePositionInWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition); 
        horizontalDistPlayerToMouse = mousePositionInWorld.x - PlayerController._instance.entityRigidbody.position.x;

        isFacingRight = horizontalDistPlayerToMouse >= 0;

        if (horizontalDistPlayerToMouse * PlayerController._instance.playerTransform.localScale.x < 0)
        {
            FlipDirecton();
        }

        armPivot.up = -(GetAimPoint() - (Vector2)armPivot.position);
    }

    private void FlipDirecton()
    {
        PlayerController._instance.Flip();

        armPivot = PlayerEquipedItemsController._instance.GetArmPivot();

        FlipSecondaryArmPivot();
    }

    private Vector2 GetAimPoint()
    {
        return mousePositionInWorld;
    }
}
