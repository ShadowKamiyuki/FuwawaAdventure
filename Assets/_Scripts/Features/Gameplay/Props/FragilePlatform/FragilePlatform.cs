using System.Collections;
using UnityEngine;

public class FragilePlatform : MonoBehaviour
{
    [Header("Timing")]
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float fallDelay = 0.3f;
    [SerializeField] private float respawnTime = 3f;

    [Header("Shake")]
    [SerializeField] private float shakeAmount = 0.05f;

    private Vector3 startPosition;
    private Quaternion startRotation;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        startPosition = transform.position;
        startRotation = transform.rotation;

        ResetPlatform();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                if (contact.normal.y < -0.5f)
                {
                    StartCoroutine(PlatformRoutine());
                    break;
                }
            }
        }
    }

    private IEnumerator PlatformRoutine()
    {
        float timer = 0f;

        while (timer < shakeDuration)
        {
            timer += Time.deltaTime;

            transform.position = startPosition +
                                 Random.insideUnitSphere * shakeAmount;

            yield return null;
        }

        transform.position = startPosition;

        yield return new WaitForSeconds(fallDelay);

        rb.isKinematic = false;

        yield return new WaitForSeconds(respawnTime);

        ResetPlatform();
    }

    private void ResetPlatform()
    {
        rb.isKinematic = true;

        transform.position = startPosition;
        transform.rotation = startRotation;
    }
}