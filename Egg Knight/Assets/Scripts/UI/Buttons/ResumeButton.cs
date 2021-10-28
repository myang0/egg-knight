using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ResumeButton : MonoBehaviour {
	private Button _button;

  public static event EventHandler OnResumeButtonPressed;

	private void Awake() {
    _button = gameObject.GetComponent<Button>();
    _button.onClick.AddListener(ResumeGame);
  }

  private void ResumeGame() {
    OnResumeButtonPressed?.Invoke(this, EventArgs.Empty);
  }
}
