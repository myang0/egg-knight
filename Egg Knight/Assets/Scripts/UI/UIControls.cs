using System;
using UnityEngine;

public class UIControls : MonoBehaviour {
	public static event EventHandler OnEscPress;

  private void FixedUpdate() {
    EscPress();
  }

  private void EscPress() {
		if (Input.GetKey(KeyCode.Escape)) {
			OnEscPress?.Invoke(this, EventArgs.Empty);
		}
	}
}
