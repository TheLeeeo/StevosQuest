using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCloudOnUse : MonoBehaviour
{
    [SerializeField]
    private GameObject CloudObject;

    private ItemCore itemCore;

    private Camera mainCamera;

    private void Awake()
    {
        itemCore = GetComponent<ItemCore>();
        itemCore.OnMainUse += Use;

        mainCamera = Camera.main;
    }

    private void Use(EntityController entityController, ClickType clickType)
    {
        if (clickType == ClickType.Press)
        {
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            Instantiate(CloudObject, mousePosition, Quaternion.identity);
        }
    }
}
