using System;
using UnityEngine;

public class PlayerControls : MonoBehaviour {
    public static event EventHandler<MovementVectorEventArgs> OnMovement;
    public static event EventHandler OnMovementKeysPressed;

    private bool _movementKeysDown = false;

    private Vector2 _lastMovementVector = Vector2.zero;

    private void Update() {
        if (_movementKeysDown != MovementKeysPressed()) {
            OnMovementKeysPressed?.Invoke(this, EventArgs.Empty);
        }

        _movementKeysDown = MovementKeysPressed();

        Vector2 movementVector = new Vector2(
            0 + (Input.GetKey(KeyCode.A) ? -1 : 0) + (Input.GetKey(KeyCode.D) ? 1 : 0),
            0 + (Input.GetKey(KeyCode.S) ? -1 : 0) + (Input.GetKey(KeyCode.W) ? 1 : 0)
        );

        if (!_lastMovementVector.Equals(movementVector)) {
            OnMovement?.Invoke(this, new MovementVectorEventArgs { vector = movementVector });
        }

        _lastMovementVector = movementVector;
    }

    private bool MovementKeysPressed() {
        return (
            Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)
        );
    }
}
