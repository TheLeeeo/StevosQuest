using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedCircleSpawner : MonoBehaviour
{
    [SerializeField]
    private float spawnRadius;

    [SerializeField]
    private float spawnTime;

    [SerializeField]
    private EntityTable enemySpawnTable;

    [SerializeField]
    private int spawnLimit;
    private int spawnedEntities = 0;

    private void Start()
    {
        Activate();
    }

    private IEnumerator SpawnTimer()
    {
        while (true)
        {            
            yield return new WaitForSeconds(spawnTime);

            if (spawnedEntities < spawnLimit && LevelController.activeEnemyCount < LevelController.maxEnemyCount)
            {
                Vector3 position = transform.position + (Vector3)Random.insideUnitCircle * spawnRadius;

                if (WorldGrid.FreeSpaceAtPosition(position))
                {
                    EntityController spawnedEnemyController = LevelController._instance.SpawnEntity(enemySpawnTable.GetEntity(), transform.position + (Vector3)Random.insideUnitCircle * spawnRadius);
                    spawnedEnemyController.deathEvent.AddListener(() => spawnedEntities--);
                    spawnedEntities++;
                }               
            }
        }
    }

    public void Activate()
    {
        StartCoroutine(SpawnTimer());
    }

    public void Deactivate()
    {
        StopCoroutine(SpawnTimer());
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
