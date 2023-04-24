using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffscreenSpawner : MonoBehaviour
{
    #if UNITY_EDITOR
    private static Transform playerTransform
    {
        get
        {
            if (_playerTransform == null)
            {
                _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;                
            }

            return _playerTransform;
        }
    }
    private static Transform _playerTransform;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireCube(playerTransform.position, spawnArea * 2);
    }
#endif

    public static OffscreenSpawner _instance;

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

    [SerializeField]
    private Vector2 spawnArea = new Vector2(20, 12);

    public float spawnTime;

    [SerializeField]
    private EntityTable entityTable;

    private void Start()
    {
        if (entityTable == null)
        {
            entityTable = LevelData.GetDefaultEntityTable();
        }

        StartCoroutine(SpawnTimer());
    }

    private IEnumerator SpawnTimer()
    {
        while (true)
        {
            float waitTime = spawnTime;

            if (LevelController.activeEnemyCount < LevelController.maxEnemyCount)
            {
                Vector3 spawnPoint = GetSpawnPoint();

                if (spawnPoint != Vector3.zero)
                {
                    LevelController._instance.SpawnEntity(entityTable.GetEntity(), spawnPoint);
                }
                else
                {
                    waitTime *= 0.1f;
                }      
            }

            yield return new WaitForSeconds(waitTime);
        }
    }

    private Vector3 GetSpawnPoint()
    {
        float x = Random.Range(-spawnArea.x, spawnArea.x);
        float y = Random.Range(-spawnArea.y, spawnArea.y);

        Vector3 spawnPosition = new Vector3(x, y, 0) + PlayerController._instance.playerTransform.position;

        //false == Physics2D.OverlapBox(spawnPosition, Vector2.one, 0, CommonLayerMasks.GroundCheckLayers)

        if (WorldGrid.FreeSpaceAtPosition(spawnPosition))
        {
            return spawnPosition;
        }

        return Vector3.zero;
    }
}
