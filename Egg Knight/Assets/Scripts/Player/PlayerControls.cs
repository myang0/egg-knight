using System;
using UnityEngine;

public class PlayerControls : MonoBehaviour {
	public static event EventHandler<MovementVectorEventArgs> OnMovement;
	public static event EventHandler OnMovementKeysPressed;
	public static event EventHandler OnRightClick;

	public static event EventHandler OnSpaceBarPressed;
	public static event EventHandler OnLeftClick;
	public static event EventHandler OnQPress;
	public static event EventHandler OnEPress;

	private bool _controlsEnabled = true;

	private bool _movementKeysDown = false;

	private void Awake() {
		PlayerMovement.OnRollBegin += DisableControl;
		PlayerMovement.OnRollEnd += EnableControl;
	}

	private void DisableControl(object sender, RollEventArgs e) {
		_controlsEnabled = false;
	}

	private void EnableControl(object sender, EventArgs e) {
		_controlsEnabled = true;
	}

	private void Update() {
		if (_controlsEnabled) {
			MovementControls();

			RollControls();

			ShootControls();
			AttackControls();
			AttackSwitchControls();
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

		if (movementVector.x != 0 && movementVector.y != 0) {
			movementVector *= 0.8f;
		}

		OnMovement?.Invoke(this, new MovementVectorEventArgs(movementVector));
	}

	private void RollControls() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			OnSpaceBarPressed?.Invoke(this, EventArgs.Empty);
		}
	}

	private void ShootControls() {
		if (Input.GetMouseButtonDown((int)MouseInput.RightClick)) {
			OnRightClick?.Invoke(this, EventArgs.Empty);
		}
	}

	private void AttackControls() {
		if (Input.GetMouseButtonDown((int)MouseInput.LeftClick)) {
			OnLeftClick?.Invoke(this, EventArgs.Empty);
		}
	}

	private void AttackSwitchControls() {
		if (Input.GetKeyDown(KeyCode.Q)) {
			OnQPress?.Invoke(this, EventArgs.Empty);
		} else if (Input.GetKeyDown(KeyCode.E)) {
			OnEPress?.Invoke(this, EventArgs.Empty);
		}
	}

	private bool MovementKeysPressed() {
		return (
			Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)
		);
	}
}
