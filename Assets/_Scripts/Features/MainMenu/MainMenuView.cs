using UnityEngine;
using UnityEngine.UI;
using System;

public class MainMenuView : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button quitButton;

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private CanvasGroup tutorialGroup;

    public event Action OnStartClicked;
    public event Action OnConfirmClicked;
    public event Action OnQuitClicked;

    private void Awake()
    {
        startButton.onClick.AddListener(() => OnStartClicked?.Invoke());
        confirmButton.onClick.AddListener(() => OnConfirmClicked?.Invoke());
        quitButton.onClick.AddListener(() => OnQuitClicked?.Invoke());
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

    public void ShowTutorial()
    {
        tutorialGroup.alpha = 1;
        tutorialGroup.interactable = true;
        tutorialGroup.blocksRaycasts = true;
    }

    public void HideTutorial()
    {
        tutorialGroup.alpha = 0;
        tutorialGroup.interactable = false;
        tutorialGroup.blocksRaycasts = false;
    }
}
