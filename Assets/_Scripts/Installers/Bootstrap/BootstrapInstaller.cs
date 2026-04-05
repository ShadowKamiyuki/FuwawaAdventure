using System.Collections.Generic;
using UnityEngine;

public class BootstrapInstaller : MonoBehaviour
{
    [Header("Global Services")]
    //[SerializeField] private AudioManager audioManager;
    [SerializeField] private CustomUpdateManager updateManager;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private SceneLoaderService sceneLoaderService;

    private void Awake()
    {
        Debug.Log("=== CoreInstaller iniciado ===");

        IInputService inputService = new InputService();
        ServiceLocator.Register(inputService);

        //RegisterService<IAudioService>(audioManager);
        RegisterService<ISceneLoader>(sceneLoaderService);
        RegisterService<IUpdateService>(updateManager);
        RegisterService<IAppStateMachine>(gameManager);

        Debug.Log("=== Servicios globales registrados ===");

        Dictionary<AppState, IAppState> states = new()
        {
            { AppState.MainMenu, new MainMenuState() },
            { AppState.Loading, new LoadingState(gameManager) },
            { AppState.Gameplay, new GameplayState() },
            { AppState.GameOver, new GameOverState() },
            { AppState.Paused, new PausedState() }
        };

        gameManager.RegisterStates(states);

        Debug.Log("=== Estados creados ===");
    }

    private void RegisterService<T>(MonoBehaviour service)
    {
        if (service == null)
        {
            Debug.LogError($"Servicio {typeof(T).Name} es null");
            return;
        }

        if (!service.TryGetComponent(out T typedService))
        {
            Debug.LogError($"{service.name} no implementa {typeof(T).Name}");
            return;
        }

        ServiceLocator.Register<T>(typedService);
    }
}
