using UnityEngine;

public class RotatingBumper : MonoBehaviour, IUpdatable
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private GameObject axis;

    private void Awake()
    {
        ServiceLocator.Get<IUpdateService>().Register(this);
    }

    public void Tick(float deltaTime)
    {
        axis.transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed, Space.World);
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
