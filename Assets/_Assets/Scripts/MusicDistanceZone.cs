using UnityEngine;

public class MusicChanger : MonoBehaviour
{
    [SerializeField] private MusicTrack targetTrack;

    [SerializeField] private Transform player;

    [SerializeField] private float activationDistance = 10f;

    private bool activated;

    private void Update()
    {
        if (activated)
            return;

        if (player == null)
            return;

        float distance = Vector3.Distance(
            transform.position,
            player.position);

        if (distance <= activationDistance)
        {
            activated = true;

            MusicManager.Instance.ChangeMusic(targetTrack);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;

        Gizmos.DrawWireSphere(
            transform.position,
            activationDistance);
    }
}