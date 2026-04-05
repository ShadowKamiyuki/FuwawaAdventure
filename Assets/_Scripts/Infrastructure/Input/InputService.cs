using System;
using UnityEngine;

public class InputService : IInputService
{
    private PlayerControls _controls;

    public Vector2 Movement => _controls.Player.Move.ReadValue<Vector2>();
    public Vector2 Look => _controls.Player.Look.ReadValue<Vector2>();

    public event Action AttackStarted;
    public event Action AttackCanceled;
    public event Action JumpStarted;
    public event Action JumpCanceled;
    public event Action InteractPressed;
    public event Action PausePressed;

    public InputService()
    {
        _controls = new PlayerControls();

        _controls.Player.Attack.started += _ => AttackStarted?.Invoke();
        _controls.Player.Attack.canceled += _ => AttackCanceled?.Invoke();
        _controls.Player.Jump.started += _ => JumpStarted?.Invoke();
        _controls.Player.Jump.canceled += _ => JumpCanceled?.Invoke();
        _controls.Player.Interact.performed += _ => InteractPressed?.Invoke();
        _controls.Player.Pause.performed += _ => PausePressed?.Invoke();

        _controls.Enable();
    }

    public void EnableGameplay()
    {
        _controls.UI.Disable();
        _controls.Player.Enable();
    }

    public void EnableUI()
    {
        _controls.Player.Disable();
        _controls.UI.Enable();
    }
}