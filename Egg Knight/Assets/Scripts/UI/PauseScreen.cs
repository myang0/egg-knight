using System;
using UnityEngine;

public class PauseScreen : MonoBehaviour {
  [SerializeField] private GameObject _overlay;

  public static event EventHandler OnGamePaused;
  public static event EventHandler OnGameResumed;

  public void Awake() {
    UIControls.OnEscPress += HandleEscPress;
    ResumeButton.OnResumeButtonPressed += HandleResumeButtonPressed;
  }

  private void HandleEscPress(object sender, EventArgs e) {
    _overlay?.SetActive(true);
    Time.timeScale = 0;

    OnGamePaused?.Invoke(this, EventArgs.Empty);
  }

  private void HandleResumeButtonPressed(object sender, EventArgs e) {
    _overlay?.SetActive(false);
    Time.timeScale = 1;

    OnGameResumed?.Invoke(this, EventArgs.Empty);
  }

  private void OnDestroy() {
    UIControls.OnEscPress -= HandleEscPress;
    ResumeButton.OnResumeButtonPressed -= HandleResumeButtonPressed;
  }
}
