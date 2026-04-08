using UnityEngine;
using System.Collections;

public class FlipperPlatform : MonoBehaviour
{
    public float velocidadRotacion = 200f;
    public float delay = 0.2f;
    private bool activada = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (!activada && collision.gameObject.CompareTag("Player"))
        {
            activada = true;
            StartCoroutine(Flip());
        }
    }

    IEnumerator Flip()
    {
        yield return new WaitForSeconds(delay);

        float rotado = 0f;
        while (rotado < 180f)
        {
            float step = velocidadRotacion * Time.deltaTime;
            transform.Rotate(Vector3.right, step);
            rotado += step;
            yield return null;
        }
    }
}