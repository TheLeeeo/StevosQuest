using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowTowardsMouse : MonoBehaviour
{
    private UtilityCore utilityCore;

    private Camera mainCamera;

    [SerializeField]
    private GameObject objectToSpawn;

    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float forceMultiplier;

    private void Awake()
    {
        utilityCore = GetComponent<UtilityCore>();

        //utilityCore.OnActivate += Activate;
        utilityCore.OnMainUse += Throw;
        //utilityCore.OnDeactivate += Deactivate;

        mainCamera = Camera.main;
    }

    private void Throw(EntityController entityController, ClickType clickType)
    {
        if (ClickType.Press == clickType)
        {
            GameObject spawnedObject = Instantiate(objectToSpawn, transform.position, Quaternion.identity);

            Vector3 relativeVector = mainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;

            float magnitude = Mathf.Clamp(relativeVector.magnitude * forceMultiplier, 0, maxSpeed);
            relativeVector = relativeVector.normalized * magnitude;

            spawnedObject.GetComponent<ThrowableController>().Initiate(relativeVector, utilityCore);
        }
    }
}

