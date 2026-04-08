using UnityEngine;

public class RotatingBumper : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    [SerializeField] private GameObject axis;

    private void Update()
    {
        axis.transform.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime, 0));
    }
}
