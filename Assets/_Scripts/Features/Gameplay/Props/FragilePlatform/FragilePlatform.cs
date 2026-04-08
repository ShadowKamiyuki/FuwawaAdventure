using System.Collections;
using UnityEngine;

public class FragilePlatform : MonoBehaviour
{
    [SerializeField] private float delay = 1f;
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private float shakeAmount = 0.05f;

    private Vector3 originalPosition;
    private bool triggered;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        originalPosition = transform.position;
    }

    public void TriggerPlatform()
    {
        if (triggered) return;
        triggered = true;
        StartCoroutine(FallRoutine());
    }

    private IEnumerator FallRoutine()
    {
        float timer = 0f;

        while (timer < shakeDuration)
        {
            timer += Time.deltaTime;
            transform.position = originalPosition + Random.insideUnitSphere * shakeAmount;
            yield return null;
        }

        transform.position = originalPosition;

        float remainingDelay = Mathf.Max(0f, delay - shakeDuration);
        yield return new WaitForSeconds(remainingDelay);
        rb.isKinematic = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TriggerPlatform();
        }
    }
}
