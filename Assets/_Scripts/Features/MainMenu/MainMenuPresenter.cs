public class MainMenuPresenter
{
    private readonly MainMenuView _view;
    private readonly IAppStateMachine _stateMachine;

    public MainMenuPresenter(MainMenuView view)
    {
        _view = view;
        _stateMachine = ServiceLocator.Get<IAppStateMachine>();
    }

    public void Initialize()
    {
        _view.OnStartClicked += OnStartClicked;
        _view.OnConfirmClicked += OnConfirmClicked;
        _view.OnQuitClicked += OnQuitClicked;
        _view.Show();
    }

    public void Dispose()
    {
        _view.OnStartClicked -= OnStartClicked;
        _view.OnConfirmClicked -= OnConfirmClicked;
        _view.OnQuitClicked -= OnQuitClicked;
        _view.Hide();
        _view.HideTutorial();
    }

    private void OnConfirmClicked()
    {
        var request = new LoadingRequest(
            load: new[] { "Game", "Pause", "GameOver" },
            unload: new[] { "MainMenu" },
            nextState: AppState.Gameplay
        );

        _stateMachine.RequestSceneChange(request);
    }

    private void OnQuitClicked()
    {
        UnityEngine.Application.Quit();
    }

    private void OnStartClicked()
    {
        _view.ShowTutorial();
    }
}
