using System;
using UnityEngine;

public interface IInputService
{
    Vector2 Movement { get; }

    event Action JumpStarted;
    event Action JumpCanceled;
    event Action PausePressed;

    void EnableGameplay();
    void EnableUI();
}