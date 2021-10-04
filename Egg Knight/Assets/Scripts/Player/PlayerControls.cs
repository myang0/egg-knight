using System;
using UnityEngine;

public class PlayerControls : MonoBehaviour {
	public static event EventHandler<MovementVectorEventArgs> OnMovement;
	public static event EventHandler OnMovementKeysPressed;
	public static event EventHandler OnRightClick;

	private bool _controlsEnabled = true;

	private bool _movementKeysDown = false;

	private Vector2 _lastMovementVector = Vector2.zero;

	private void Awake() {
		PlayerMovement.OnRollBegin += DisableControl;
		PlayerMovement.OnRollEnd += EnableControl;
	}

	private void DisableControl(object sender, RollEventArgs e) {
		_controlsEnabled = false;
		_lastMovementVector = Vector2.zero;
	}

	private void EnableControl(object sender, EventArgs e) {
		_controlsEnabled = true;
	}

	private void Update() {
		if (_controlsEnabled) {
			MovementControls();

			RollControls();
		}
	}

	private void MovementControls() {
		if (_movementKeysDown != MovementKeysPressed()) {
			OnMovementKeysPressed?.Invoke(this, EventArgs.Empty);
		}

		_movementKeysDown = MovementKeysPressed();

		Vector2 movementVector = new Vector2(
			0 + (Input.GetKey(KeyCode.A) ? -1 : 0) + (Input.GetKey(KeyCode.D) ? 1 : 0),
			0 + (Input.GetKey(KeyCode.S) ? -1 : 0) + (Input.GetKey(KeyCode.W) ? 1 : 0)
		);

		if (!_lastMovementVector.Equals(movementVector)) {
			OnMovement?.Invoke(this, new MovementVectorEventArgs(movementVector));
		}

		_lastMovementVector = movementVector;
	}

	private void RollControls() {
		if (Input.GetMouseButtonDown((int)MouseInput.RightClick)) {
			OnRightClick?.Invoke(this, EventArgs.Empty);
		}
	}

	private bool MovementKeysPressed() {
		return (
			Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)
		);
	}
}
