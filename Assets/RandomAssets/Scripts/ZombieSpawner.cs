using UnityEngine;
using System.Collections.Generic;

public class HunterMonsterSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private List<GameObject> zombiePrefabs;
    [SerializeField] private GameObject player;

    [Space(10)]
    [Header("Spawn Area")]
    [SerializeField] private Vector2 spawnAreaSize = new Vector2(10f, 10f);

    [Space(10)]
    [Header("Gizmos")]
    [SerializeField] private bool showGizmos = true;
    [SerializeField] private Color gizmoColor = Color.green;

    public void SpawnZombie()
    {
        #region Validation

        if (zombiePrefabs == null || zombiePrefabs.Count == 0)
        {
            Debug.LogError("Zombie prefab list is empty.");
            return;
        }

        if (player == null)
        {
            Debug.LogError("Player is not assigned.");
            return;
        }

        #endregion

        #region Pick Random Prefab

        int randomIndex = Random.Range(0, zombiePrefabs.Count);
        GameObject selectedZombie = zombiePrefabs[randomIndex];

        if (selectedZombie == null)
        {
            Debug.LogError("Selected zombie prefab is null.");
            return;
        }

        #endregion

        #region Get Random Position

        Vector3 randomOffset = new Vector3(
            Random.Range(-spawnAreaSize.x / 2f, spawnAreaSize.x / 2f),
            0f,
            Random.Range(-spawnAreaSize.y / 2f, spawnAreaSize.y / 2f)
        );

        Vector3 spawnPosition = transform.position + randomOffset;

        #endregion

        #region Spawn Zombie

        GameObject spawnedZombie = Instantiate(selectedZombie, spawnPosition, Quaternion.identity);

        #endregion

        #region Handle NPCPatrolPath

        NPCPatrolPath patrolPath = spawnedZombie.GetComponentInChildren<NPCPatrolPath>();

        if (patrolPath != null)
        {
            Transform patrolTransform = patrolPath.transform;

            // Move it under player
            patrolTransform.SetParent(player.transform);

            // Reset local position
            patrolTransform.localPosition = Vector3.zero;
        }
        else
        {
            Debug.LogWarning("No NPCPatrolPath found in spawned zombie.");
        }

        #endregion
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos) return;

        Gizmos.color = gizmoColor;

        Vector3 size = new Vector3(spawnAreaSize.x, 0.1f, spawnAreaSize.y);
        Gizmos.DrawWireCube(transform.position, size);
    }
}