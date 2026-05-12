using UnityEngine;

public class RespawnService : IRespawnService
{
    private Vector3 _position;
    private Quaternion _rotation;

    public void SetCheckpoint(Vector3 position, Quaternion rotation)
    {
        _position = position;
        _rotation = rotation;
    }

    public (Vector3, Quaternion) GetCheckpoint()
    {
        return (_position, _rotation);
    }
}