using UnityEngine;
using System.Collections;

public class FlipperPlatform : MonoBehaviour
{
    public float RotationSpeed = 200f;
    public float delay = 0.2f;
    private bool activated = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (!activated && collision.gameObject.CompareTag("Player"))
        {
            activated = true;
            StartCoroutine(Flip());
        }
    }

    IEnumerator Flip()
    {
        yield return new WaitForSeconds(delay);

        float rotado = 0f;
        while (rotado < 180f)
        {
            float step = RotationSpeed * Time.deltaTime;
            transform.Rotate(Vector3.right, step);
            rotado += step;
            activated = false;
            yield return null;
        }
    }
}