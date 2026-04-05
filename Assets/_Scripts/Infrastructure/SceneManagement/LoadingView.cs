using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class LoadingView : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Slider progressBar;

    [Header("Fade")]
    [SerializeField] private FadeController fadeController;

    private void Awake()
    {
        ServiceLocator.Register<LoadingView>(this);

        if (progressBar != null)
        {
            progressBar.minValue = 0f;
            progressBar.maxValue = 1f;
            progressBar.value = 0f;
        }
    }

    private void OnDestroy()
    {
        ServiceLocator.Unregister<LoadingView>();
    }

    public void SetProgress(float value)
    {
        if (progressBar != null)
            progressBar.value = value;
    }

    public void ShowLoadingScreen()
    {
        gameObject.SetActive(true);

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }

    public void HideLoadingScreen()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;

        gameObject.SetActive(false);
    }

    public async Task FadeInAsync()
    {
        if (fadeController != null)
        {
            ShowLoadingScreen();
            await fadeController.FadeInAsync();
        }
    }

    public async Task FadeOutAsync()
    {
        if (fadeController != null)
        {
            HideLoadingScreen();
            await fadeController.FadeOutAsync();
        }
    }
}