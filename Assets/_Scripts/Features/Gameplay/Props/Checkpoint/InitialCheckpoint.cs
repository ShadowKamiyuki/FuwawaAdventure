using UnityEngine;

public class InitialCheckpoint : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;

    private void Start()
    {
        IRespawnService respawnService = ServiceLocator.Get<IRespawnService>();

        respawnService.SetCheckpoint(
            spawnPoint.position,
            spawnPoint.rotation
        );
    }
}