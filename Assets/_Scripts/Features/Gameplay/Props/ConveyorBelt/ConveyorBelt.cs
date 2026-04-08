using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public Vector3 direccion = Vector3.forward;
    public float velocidad = 3f;

    private void OnCollisionStay(Collision collision)
    {
        if (collision.rigidbody != null)
        {
            collision.rigidbody.AddForce(direccion * velocidad, ForceMode.Acceleration);
        }
    }
}