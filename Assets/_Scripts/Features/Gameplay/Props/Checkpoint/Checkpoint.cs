using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private GameObject vfx;
    [SerializeField] private AudioSource audioSource;

    private bool _activated;

    private void OnTriggerEnter(Collider other)
    {
        if (_activated) return;
        if (!other.CompareTag("Player")) return;

        IRespawnService respawnService = ServiceLocator.Get<IRespawnService>();
        respawnService.SetCheckpoint(respawnPoint.position, respawnPoint.rotation);

        ActivateFeedback();
        _activated = true;
    }

    private void ActivateFeedback()
    {
        if (vfx != null)
            Instantiate(vfx, transform.position, Quaternion.identity);

        if (audioSource != null)
            audioSource.Play();

        // ejemplo animación
        GetComponent<Animator>().SetTrigger("Activate");
    }
}