using UnityEngine;

public interface IRespawnService
{
    void SetCheckpoint(Vector3 position, Quaternion rotation);
    (Vector3, Quaternion) GetCheckpoint();
}