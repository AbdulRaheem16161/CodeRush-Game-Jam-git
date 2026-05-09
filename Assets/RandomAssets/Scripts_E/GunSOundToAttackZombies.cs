using System.Collections.Generic;
using UnityEngine;
using Akila.FPSFramework;
using AbdulRaheem.Game.NPC;

public class GunSoundToAttackZombies : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Firearm fireArm;

    [Header("Sound Settings")]
    [SerializeField] private float soundRadius = 15f;
    [SerializeField] private LayerMask zombieLayer;
    [SerializeField] private float alertMemoryTime = 3f;

    [Header("Gizmos Settings")]
    [SerializeField] private bool showGizmos = true;
    [SerializeField] private Color normalColor = Color.green;
    [SerializeField] private Color alertColor = Color.red;
    [SerializeField] private float alertFlashDuration = 0.2f;

    [Header("Debug - Alerted NPCs")]
    [SerializeField] private List<NPCEntry> alertedNPCs = new List<NPCEntry>();

    public AudioSource gunShotSound;

    public bool soundTriggered;
    private float alertTimer;

    [System.Serializable]
    public class NPCEntry
    {
        public NPCSight npc;
        public float timer;
    }

    private void Awake()
    {
        if (fireArm == null)
            fireArm = GetComponent<Firearm>();
    }

    private void OnEnable()
    {
        if (fireArm != null)
            fireArm.OnShoot += CreateSoundToAttackZombies;
    }

    private void OnDisable()
    {
        if (fireArm != null)
            fireArm.OnShoot -= CreateSoundToAttackZombies;
    }

    private void Update()
    {
        // Flash gizmo timer
        if (soundTriggered)
        {
            alertTimer -= Time.deltaTime;

            if (alertTimer <= 0f)
                soundTriggered = false;
        }

        // Update alerted NPC list timers
        for (int i = alertedNPCs.Count - 1; i >= 0; i--)
        {
            alertedNPCs[i].timer -= Time.deltaTime;

            if (alertedNPCs[i].timer <= 0f)
            {
                alertedNPCs.RemoveAt(i);
            }
        }
    }

    public void CreateSoundToAttackZombies()
    {
        Debug.Log("CreateSoundToAttackZombies");
        Collider[] hits = Physics.OverlapSphere(transform.position, soundRadius, zombieLayer);

        foreach (Collider hit in hits)
        {
            NPCSight npcSight = hit.GetComponent<NPCSight>();

            if (npcSight != null)
            {
                npcSight.HearShound(0f);

                AddOrRefreshNPC(npcSight);
            }
        }

        soundTriggered = true;
        gunShotSound.Play();
        alertTimer = alertFlashDuration;
    }

    private void AddOrRefreshNPC(NPCSight npc)
    {
        // check if already exists
        for (int i = 0; i < alertedNPCs.Count; i++)
        {
            if (alertedNPCs[i].npc == npc)
            {
                alertedNPCs[i].timer = alertMemoryTime;
                return;
            }
        }

        // add new entry
        alertedNPCs.Add(new NPCEntry
        {
            npc = npc,
            timer = alertMemoryTime
        });
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos) return;

        Gizmos.color = soundTriggered ? alertColor : normalColor;
        Gizmos.DrawWireSphere(transform.position, soundRadius);
    }
}