using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioClip gameplayMusic;

    private IAppStateMachine gameStateMachine;

    private void Start()
    {
        gameStateMachine = ServiceLocator.Get<IAppStateMachine>();
        gameStateMachine.OnStateChanged += HandleStateChanged;
    }

    private void HandleStateChanged(AppState state)
    {
        switch (state)
        {
            case AppState.MainMenu:
                musicSource.Pause();
                break;

            case AppState.Gameplay:
                PlayMusic(gameplayMusic);
                break;

            case AppState.Paused:
                musicSource.Pause();
                break;
        }
    }

    private void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip)
        {
            musicSource.UnPause();
            return;
        }

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }
}
