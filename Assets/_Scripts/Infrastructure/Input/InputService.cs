using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputService : IInputService, IDisposable
{
    private PlayerControls _controls;

    public event Action AttackStarted;
    public event Action AttackCanceled;
    public event Action JumpStarted;
    public event Action JumpCanceled;
    public event Action InteractPressed;
    public event Action PausePressed;

    private Vector2 _smoothLook;
    private Vector2 _lookVelocity;

    public InputService()
    {
        _controls = new PlayerControls();

        _controls.Player.Attack.started += OnAttackStarted;
        _controls.Player.Attack.canceled += OnAttackCanceled;
        _controls.Player.Jump.started += OnJumpStarted;
        _controls.Player.Jump.canceled += OnJumpCanceled;
        _controls.Player.Interact.performed += OnInteract;
        _controls.Player.Pause.performed += OnPause;

        _controls.Enable();
    }

    public Vector2 Movement => _controls.Player.Move.ReadValue<Vector2>();

    public Vector2 GetLook(bool useDeltaTime = true)
    {
        Vector2 input = _controls.Player.Look.ReadValue<Vector2>();

        var device = _controls.Player.Look.activeControl?.device;

        if (device != null && device.name.Contains("Gamepad"))
            return input * (useDeltaTime ? Time.deltaTime : 1f);

        return input;
    }

    public Vector2 GetSmoothLook(float smoothTime)
    {
        Vector2 raw = GetLook();
        _smoothLook = Vector2.SmoothDamp(_smoothLook, raw, ref _lookVelocity, smoothTime);
        return _smoothLook;
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

    public void Dispose()
    {
        _controls.Player.Attack.started -= OnAttackStarted;
        _controls.Player.Attack.canceled -= OnAttackCanceled;
        _controls.Player.Jump.started -= OnJumpStarted;
        _controls.Player.Jump.canceled -= OnJumpCanceled;
        _controls.Player.Interact.performed -= OnInteract;
        _controls.Player.Pause.performed -= OnPause;

        _controls.Dispose();
    }

    private void OnAttackStarted(InputAction.CallbackContext _) => AttackStarted?.Invoke();
    private void OnAttackCanceled(InputAction.CallbackContext _) => AttackCanceled?.Invoke();
    private void OnJumpStarted(InputAction.CallbackContext _) => JumpStarted?.Invoke();
    private void OnJumpCanceled(InputAction.CallbackContext _) => JumpCanceled?.Invoke();
    private void OnInteract(InputAction.CallbackContext _) => InteractPressed?.Invoke();
    private void OnPause(InputAction.CallbackContext _) => PausePressed?.Invoke();
}