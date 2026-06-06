using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
  private PlayerControls controls;

  public event Action<Vector2> OnMoveInputChanged;
  public event Action OnJumpPressed;

  private void Awake()
  {
    controls = new PlayerControls();

    controls.Player.Move.performed += ctx => SendMoveInput(ctx.ReadValue<Vector2>());
    controls.Player.Move.canceled += ctx => SendMoveInput(Vector2.zero);

    controls.Player.Jump.performed += ctx => TriggerJump();
  }

  private void OnEnable() => controls.Player.Enable();

  private void OnDisable() => controls.Player.Disable();

  private void SendMoveInput(Vector2 inputDirection)
  {
    OnMoveInputChanged?.Invoke(inputDirection);
  }

  private void TriggerJump()
  {
    OnJumpPressed?.Invoke();
  }
}
