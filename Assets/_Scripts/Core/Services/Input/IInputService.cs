using System;
using UnityEngine;

public interface IInputService
{
    Vector2 Movement { get; }
    Vector2 Look { get; }

    event Action AttackStarted;
    event Action AttackCanceled;
    event Action JumpStarted;
    event Action JumpCanceled;
    event Action InteractPressed;
    event Action PausePressed;

    void EnableGameplay();
    void EnableUI();
}