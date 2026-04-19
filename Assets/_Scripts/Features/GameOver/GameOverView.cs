using UnityEngine;
using UnityEngine.UI;
using System;

public class GameOverView : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Button backToMenuButton;

    public event Action OnBackToMenu;

    private void Awake()
    {
        ServiceLocator.Register(this);
        backToMenuButton.onClick.AddListener(() => OnBackToMenu?.Invoke());
        Hide();
    }

    private void OnDestroy()
    {
        if (ServiceLocator.Exists<GameOverView>())
            ServiceLocator.Unregister<GameOverView>();
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
