using System.Collections.Generic;
using UnityEngine;

public class RandomBodySpawner : MonoBehaviour
{
    #region Prefab Settings

    [SerializeField] private List<GameObject> prefabs = new List<GameObject>();

    #endregion

    [SerializeField] private float scaleMultiplyer;

    [SerializeField] private GameObject instance;

    [SerializeField] private bool turnOff;

    private void Awake()
    {
        #region Awake

        if (turnOff) return;
        SpawnRandomChild();

        #endregion
    }

    private void Update()
    {
        if (instance != null)
        {
            instance.transform.localRotation = Quaternion.identity;
        }
    }

    private void SpawnRandomChild()
    {
        #region Spawn Random Child

        if (prefabs == null || prefabs.Count == 0)
        {
            Debug.LogWarning("No prefabs assigned to RandomChildSpawner.");
            return;
        }

        int randomIndex = Random.Range(0, prefabs.Count);
        GameObject selectedPrefab = prefabs[randomIndex];

        instance = Instantiate(selectedPrefab);

        instance.transform.SetParent(transform);
        instance.transform.localPosition = Vector3.zero;
        instance.transform.localRotation = Quaternion.identity;
        instance.transform.localScale = Vector3.one * scaleMultiplyer;

        RemovePhysicsComponents(instance);

        #endregion
    }

    private void RemovePhysicsComponents(GameObject obj)
    {
        #region Remove Rigidbody Only

        // Get ALL rigidbodies (including children)
        Rigidbody[] rigidbodies = obj.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rb in rigidbodies)
        {
            Destroy(rb);
        }

        #endregion
    }
}