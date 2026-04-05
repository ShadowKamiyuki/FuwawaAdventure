using UnityEngine;

public class GameplayState : IAppState
{
    public void Enter()
    {
        Time.timeScale = 1f;
    }

    public void Exit() { }
}