using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private int numberToSpawn = 5;
    [SerializeField] private Vector3 spawnAreaSize = new Vector3(10f, 0f, 10f);
    [SerializeField] private float checkRadius = 1.5f;
    [SerializeField] private LayerMask occupiedLayer;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private int maxTotalAttempts = 200;

    private void Awake()
    {
        SpawnAllImmediately();
    }

    private void SpawnAllImmediately()
    {
        if (prefabs == null || prefabs.Length == 0)
        {
            Debug.LogWarning("Spawner: No prefabs assigned.", this);
            return;
        }

        int spawnedCount = 0;
        int totalAttempts = 0;

        while (spawnedCount < numberToSpawn && totalAttempts < maxTotalAttempts)
        {
            if (TrySpawnOne())
            {
                spawnedCount++;
            }

            totalAttempts++;
        }

        if (spawnedCount < numberToSpawn)
        {
            Debug.LogWarning($"Spawner: Only spawned {spawnedCount} out of {numberToSpawn}. Area may be too small or blocked.", this);
        }
    }

    private bool TrySpawnOne()
    {
        Vector3 randomPos = GetRandomPosition();

        if (Physics.CheckSphere(randomPos, checkRadius, occupiedLayer, QueryTriggerInteraction.Collide))
            return false;

        if (Physics.CheckSphere(randomPos, checkRadius, obstacleLayer, QueryTriggerInteraction.Collide))
            return false;

        GameObject prefabToSpawn = prefabs[Random.Range(0, prefabs.Length)];
        if (prefabToSpawn == null)
            return false;

        Instantiate(prefabToSpawn, randomPos, Quaternion.identity);
        return true;
    }

    private Vector3 GetRandomPosition()
    {
        return transform.position + new Vector3(
            Random.Range(-spawnAreaSize.x * 0.5f, spawnAreaSize.x * 0.5f),
            spawnAreaSize.y,
            Random.Range(-spawnAreaSize.z * 0.5f, spawnAreaSize.z * 0.5f)
        );
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, spawnAreaSize);
    }
}