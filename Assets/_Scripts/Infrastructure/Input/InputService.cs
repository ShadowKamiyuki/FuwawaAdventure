using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputService : IInputService, IDisposable
{
    private PlayerControls _controls;

    public event Action JumpStarted;
    public event Action JumpCanceled;
    public event Action PausePressed;

    public InputService()
    {
        _controls = new PlayerControls();

        _controls.Player.Jump.started += OnJumpStarted;
        _controls.Player.Jump.canceled += OnJumpCanceled;
        _controls.Player.Pause.performed += OnPause;

        _controls.Enable();
    }

    public Vector2 Movement => _controls.Player.Move.ReadValue<Vector2>();

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

    public void Dispose()
    {
        _controls.Player.Jump.started -= OnJumpStarted;
        _controls.Player.Jump.canceled -= OnJumpCanceled;
        _controls.Player.Pause.performed -= OnPause;

        _controls.Dispose();
    }

    private void OnJumpStarted(InputAction.CallbackContext _) => JumpStarted?.Invoke();
    private void OnJumpCanceled(InputAction.CallbackContext _) => JumpCanceled?.Invoke();
    private void OnPause(InputAction.CallbackContext _) => PausePressed?.Invoke();
}