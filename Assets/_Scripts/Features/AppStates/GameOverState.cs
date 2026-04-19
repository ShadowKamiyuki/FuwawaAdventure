using UnityEngine;

public class GameOverState : IAppState
{
    private GameOverPresenter _presenter;
    private GameOverView _view;

    public void Enter()
    {
        Time.timeScale = 0f;

        _view = ServiceLocator.Get<GameOverView>();

        if (_view == null)
        {
            Debug.LogError("GameOverView no encontrada.");
            return;
        }

        _presenter = new GameOverPresenter(_view);
        _presenter.Initialize();
    }

    public void Exit()
    {
        Time.timeScale = 1f;

        _presenter?.Dispose();
        _presenter = null;
    }
}
