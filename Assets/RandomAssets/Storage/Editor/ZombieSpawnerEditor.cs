using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(HunterMonsterSpawner))]
public class ZombieSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        HunterMonsterSpawner spawner = (HunterMonsterSpawner)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Spawn Zombie"))
        {
            spawner.SpawnZombie();
        }
    }
}