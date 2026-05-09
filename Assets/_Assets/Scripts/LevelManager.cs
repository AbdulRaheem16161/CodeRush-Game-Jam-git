using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    [Header("Enemies")]
    public List<GameObject> enemyList = new List<GameObject>();

    [Header("Scene Changer")]
    public GameObject sceneChanger;

    [Header("Settings")]
    [Range(0f, 1f)]
    public float requiredDeadPercentage = 0.7f; // 70%

    public bool activated = false;

    public int totalEnemies = 0;
    public int deadEnemies = 0;

    public float deadPercentage = 0f;

    private void Start()
    {
        sceneChanger.SetActive(false);
    }

    private void Update()
    {
        if (activated)
            return;

        totalEnemies = 0;
        deadEnemies = 0;

        foreach (GameObject enemy in enemyList)
        {
            // Ignore missing references
            if (enemy == null)
                continue;

            totalEnemies++;

            if (!enemy.activeInHierarchy)
            {
                deadEnemies++;
            }
        }

        if (totalEnemies == 0)
            return;

        deadPercentage = (float)deadEnemies / totalEnemies;

        if (deadPercentage >= requiredDeadPercentage)
        {
            activated = true;

            sceneChanger.SetActive(true);

            Debug.Log("70% of enemies defeated!");
        }
    }
}