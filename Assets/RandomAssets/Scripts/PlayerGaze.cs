using UnityEngine;

public class PlayerGaze : MonoBehaviour
{
    [Header("Settings")]
    public float maxDistance = 20f;
    public Camera playerCamera;

    private ReactWhenBeingWatched _currentWatched;

    void Update()
    {
        ReactWhenBeingWatched[] allCubes = FindObjectsOfType<ReactWhenBeingWatched>();

        ReactWhenBeingWatched visibleCube = null;

        foreach (ReactWhenBeingWatched cube in allCubes)
        {
            if (IsVisible(cube))
            {
                visibleCube = cube;
                break;
            }
        }

        if (visibleCube != _currentWatched)
        {
            if (_currentWatched != null)
                _currentWatched.IsBeingWatched = false;

            _currentWatched = visibleCube;

            if (_currentWatched != null)
                _currentWatched.IsBeingWatched = true;
        }
    }

    bool IsVisible(ReactWhenBeingWatched cube)
    {
        float distance = Vector3.Distance(playerCamera.transform.position, cube.transform.position);
        if (distance > maxDistance) return false;

        // Get ALL renderers on the NPC (body, head, arms, etc.)
        Renderer[] renderers = cube.GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0) return false;

        // Combine all renderers into one big bounding box
        Bounds bounds = renderers[0].bounds;
        foreach (Renderer r in renderers)
        {
            bounds.Encapsulate(r.bounds);
        }

        // Same 8 corners check as before
        Vector3[] corners = new Vector3[8];
        corners[0] = new Vector3(bounds.min.x, bounds.min.y, bounds.min.z);
        corners[1] = new Vector3(bounds.min.x, bounds.min.y, bounds.max.z);
        corners[2] = new Vector3(bounds.min.x, bounds.max.y, bounds.min.z);
        corners[3] = new Vector3(bounds.min.x, bounds.max.y, bounds.max.z);
        corners[4] = new Vector3(bounds.max.x, bounds.min.y, bounds.min.z);
        corners[5] = new Vector3(bounds.max.x, bounds.min.y, bounds.max.z);
        corners[6] = new Vector3(bounds.max.x, bounds.max.y, bounds.min.z);
        corners[7] = new Vector3(bounds.max.x, bounds.max.y, bounds.max.z);

        foreach (Vector3 corner in corners)
        {
            if (IsPointVisible(corner)) return true;
        }

        return false;
    }

    bool IsPointVisible(Vector3 worldPoint)
    {
        Vector3 screenPoint = playerCamera.WorldToViewportPoint(worldPoint);

        // Check if point is on screen
        bool inFrustum = screenPoint.z > 0 &&
                         screenPoint.x > 0 && screenPoint.x < 1 &&
                         screenPoint.y > 0 && screenPoint.y < 1;

        if (!inFrustum) return false;

        // Raycast to check if that corner is blocked by something
        Vector3 dirToPoint = worldPoint - playerCamera.transform.position;
        float dist = dirToPoint.magnitude;

        if (Physics.Raycast(playerCamera.transform.position, dirToPoint.normalized, out RaycastHit hit, dist))
        {
            if (hit.collider.GetComponent<ReactWhenBeingWatched>() == null)
                return false; // blocked by a wall
        }

        return true;
    }
}