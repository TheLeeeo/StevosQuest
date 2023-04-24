using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAndRollTowardsMouse : MonoBehaviour
{
    private UtilityCore utilityCore;

    private Camera mainCamera;

    [SerializeField]
    private GameObject objectToSpawn;

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

            Vector2 direction = Vector2.right * Mathf.Sign(relativeVector.x);

            spawnedObject.GetComponent<RollingSpikeBallController>().Initiate(direction, utilityCore);
        }
    }
}
