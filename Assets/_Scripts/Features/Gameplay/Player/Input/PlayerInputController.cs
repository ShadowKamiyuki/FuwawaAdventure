using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    private IInputService _input;
    private PlayerActions _actions;

    private void Awake()
    {
        _input = ServiceLocator.Get<IInputService>();
        _actions = GetComponent<PlayerActions>();
    }

    private void OnEnable()
    {
        _input.JumpStarted += OnJumpStarted;
        _input.JumpCanceled += OnJumpCanceled;
    }

    private void OnDisable()
    {
        _input.JumpStarted -= OnJumpStarted;
        _input.JumpCanceled -= OnJumpCanceled;
    }

    private void FixedUpdate()
    {
        _actions.SetMoveDirection(_input.Movement);
    }

    private void OnJumpStarted()
    {
        _actions.RequestJump();
        _actions.SetJumpPressed(true);
    }

    private void OnJumpCanceled()
    {
        _actions.SetJumpPressed(false);
    }
}
