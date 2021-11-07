using System;
using UnityEngine;

public class UIControls : MonoBehaviour {
	public static event EventHandler OnEscPress;

	public static event EventHandler OnTabHold;
	public static event EventHandler OnTabRelease;

  private void Update() {
    EscPress();

		if (Input.GetKeyDown(KeyCode.Tab)) {
			OnTabHold?.Invoke(this, EventArgs.Empty);
		}

		if (Input.GetKeyUp(KeyCode.Tab)) {
			OnTabRelease?.Invoke(this, EventArgs.Empty);
		}
  }

  private void EscPress() {
		if (Input.GetKey(KeyCode.Escape)) {
			OnEscPress?.Invoke(this, EventArgs.Empty);
		}
	}
}
