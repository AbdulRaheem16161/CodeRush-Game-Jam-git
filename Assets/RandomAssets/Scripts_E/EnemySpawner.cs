using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    #region Spawn Settings

    [Header("Spawn Area (Rectangle)")]
    [SerializeField] private Vector2 areaSize = new Vector2(10f, 10f);

    [Header("Spawn Rate")]
    [SerializeField] private int spawnPerMinute = 30;

    #endregion

    #region Prefab Settings

    [System.Serializable]
    public class PrefabEntry
    {
        public GameObject prefab;

        [Range(0, 100)]
        public float chance;
    }

    [SerializeField] private List<PrefabEntry> prefabs = new List<PrefabEntry>();

    #endregion

    #region Runtime

    private float spawnInterval;

    #endregion

    [SerializeField] bool turnOff;

    private void Start()
    {
        #region Start

        if (turnOff) return;

        spawnInterval = 60f / spawnPerMinute;
        StartCoroutine(SpawnRoutine());

        #endregion
    }

    private IEnumerator SpawnRoutine()
    {
        #region Spawn Routine

        while (true)
        {
            Spawn();
            yield return new WaitForSeconds(spawnInterval);
        }

        #endregion
    }

    private void Spawn()
    {
        #region Spawn

        GameObject prefabToSpawn = GetRandomPrefab();

        if (prefabToSpawn == null)
            return;

        Vector3 randomPosition = GetRandomPosition();

        Instantiate(prefabToSpawn, randomPosition, Quaternion.identity);

        #endregion
    }

    private GameObject GetRandomPrefab()
    {
        #region Weighted Random Selection

        float totalWeight = 0f;

        foreach (var entry in prefabs)
        {
            totalWeight += entry.chance;
        }

        float randomValue = Random.Range(0f, totalWeight);

        float current = 0f;

        foreach (var entry in prefabs)
        {
            current += entry.chance;

            if (randomValue <= current)
            {
                return entry.prefab;
            }
        }

        return null;

        #endregion
    }

    private Vector3 GetRandomPosition()
    {
        #region Random Position

        float x = Random.Range(-areaSize.x / 2f, areaSize.x / 2f);
        float z = Random.Range(-areaSize.y / 2f, areaSize.y / 2f);

        return transform.position + new Vector3(x, 0f, z);

        #endregion
    }

    private void OnDrawGizmosSelected()
    {
        #region Gizmos

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(areaSize.x, 0f, areaSize.y));

        #endregion
    }
}