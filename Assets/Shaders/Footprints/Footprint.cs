
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Footprint : MonoBehaviour
{
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private float fadeDuration = 1.5f;

    private DecalProjector decal;
    private Coroutine fadeRoutine;

    private void Awake()
    {
        decal = GetComponent<DecalProjector>();
    }

    private void OnEnable()
    {
        if (decal == null)
            decal = GetComponent<DecalProjector>();

        decal.fadeFactor = 1f;

        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(lifetime);

        float elapsed = 0f;
        float initialFade = decal.fadeFactor;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / fadeDuration;
            decal.fadeFactor = Mathf.Lerp(initialFade, 0f, t);

            yield return null;
        }

        decal.fadeFactor = 0f;
        gameObject.SetActive(false);

        fadeRoutine = null;
    }
}
