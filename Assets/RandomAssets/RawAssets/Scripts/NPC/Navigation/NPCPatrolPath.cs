using UnityEngine;

public class NPCPatrolPath : MonoBehaviour
{
    [SerializeField] private Transform[] trackPoints;
    public Transform[] TrackPoints => trackPoints;

    [SerializeField] private Color trackColor;
    [SerializeField] private bool showGizmos;

    private void OnDrawGizmos()
    {
        if (!showGizmos) return;

        int trackCount = trackPoints.Length;

        for (int i = 0; i < trackCount-1; i++)
        {
            Gizmos.color = trackColor;
            Gizmos.DrawLine(trackPoints[i].position,  trackPoints[i+1].position);
        }

        Gizmos.DrawLine(trackPoints[trackCount - 1].position, trackPoints[0].position);
    }

}
