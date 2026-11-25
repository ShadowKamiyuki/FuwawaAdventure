
using UnityEngine;

public class FootstepController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerActions player;
    [SerializeField] private Transform leftFoot;
    [SerializeField] private Transform rightFoot;

    [Header("Footprint")]
    [SerializeField] private GameObject footprintPrefab;
    [SerializeField] private LayerMask groundLayer;

    [Header("Settings")]
    [SerializeField] private float walkSpeedThreshold = 0.5f;
    [SerializeField] private float stepInterval = 0.4f;
    [SerializeField] private float raycastDistance = 1f;
    [SerializeField] private float groundOffset = 0.02f;

    private float stepTimer;
    private bool useLeftFoot = true;

    private void Update()
    {
        if (!CanCreateFootstep())
        {
            stepTimer = 0f;
            return;
        }

        stepTimer += Time.deltaTime;

        if (stepTimer >= stepInterval)
        {
            stepTimer = 0f;

            Transform foot = useLeftFoot ? leftFoot : rightFoot;

            CreateFootprint(foot);

            useLeftFoot = !useLeftFoot;
        }
    }

    private bool CanCreateFootstep()
    {
        return player != null &&
               player.IsGrounded &&
               player.IsMoving &&
               player.HorizontalSpeed > walkSpeedThreshold;
    }

    private void CreateFootprint(Transform foot)
    {
        if (foot == null || footprintPrefab == null)
            return;

        if (Physics.Raycast(
            foot.position,
            Vector3.down,
            out RaycastHit hit,
            raycastDistance,
            groundLayer))
        {
            Vector3 position =
                hit.point + hit.normal * groundOffset;

            Vector3 forward =
                Vector3.ProjectOnPlane(foot.forward, hit.normal).normalized;

            if (forward.sqrMagnitude < 0.001f)
                forward = Vector3.forward;

            Quaternion rotation =
                Quaternion.LookRotation(-hit.normal, forward);

            Instantiate(
                footprintPrefab,
                position,
                rotation
            );
        }
    }
}
