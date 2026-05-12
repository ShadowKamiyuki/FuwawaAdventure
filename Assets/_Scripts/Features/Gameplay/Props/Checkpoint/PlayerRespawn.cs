using UnityEngine;

public class PlayerRespawn : MonoBehaviour, IUpdatable
{
    [SerializeField] private float gameOverHeight = -10f;

    private IRespawnService _respawnService;

    private void Awake()
    {
        ServiceLocator.Get<IUpdateService>().Register(this);
    }

    private void Start()
    {
        _respawnService = ServiceLocator.Get<IRespawnService>();
    }

    public void Tick(float deltaTime)
    {
        if (transform.position.y < gameOverHeight)
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        (Vector3 pos, Quaternion rot) = _respawnService.GetCheckpoint();
        transform.SetPositionAndRotation(pos, rot);
    }

    private void OnDestroy()
    {
        IUpdateService updateManager = ServiceLocator.Get<IUpdateService>();
        if (updateManager != null)
        {
            updateManager.Unregister(this);
        }
    }
}