using UnityEngine;
using UnityEngine.UI;
using System;

public class PauseView : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button quitToMenuButton;

    public event Action OnResume;
    public event Action OnQuit;

    private void Awake()
    {
        ServiceLocator.Register(this);

        resumeButton.onClick.AddListener(() => OnResume?.Invoke());
        quitToMenuButton.onClick.AddListener(() => OnQuit?.Invoke());

        Hide();
    }

    private void OnDestroy()
    {
        if (ServiceLocator.Exists<PauseView>())
            ServiceLocator.Unregister<PauseView>();
    }

    public void Show()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void Hide()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
