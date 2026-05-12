using UnityEngine;
using UnityEngine.Playables;

public class IntroManager : MonoBehaviour
{
    public PlayableDirector timeline;

    [Header("Cameras")]
    public GameObject introCamera;
    public GameObject gameplayCamera;

    [Header("Gameplay")]
    public PlayerActions player;
    public ThirdPersonCamera gameplayCam;

    private IInputService input;

    void Start()
    {
        input = ServiceLocator.Get<IInputService>();

        // Desactivar gameplay
        input.EnableUI();
        player.enabled = false;
        gameplayCam.enabled = false;

        // Cámara intro
        introCamera.SetActive(true);
        gameplayCamera.SetActive(false);

        timeline.Play();
        timeline.stopped += OnIntroFinished;
    }

    void OnIntroFinished(PlayableDirector director)
    {
        // Activar gameplay
        introCamera.SetActive(false);
        gameplayCamera.SetActive(true);

        input.EnableGameplay();
        player.enabled = true;
        gameplayCam.enabled = true;
    }
}