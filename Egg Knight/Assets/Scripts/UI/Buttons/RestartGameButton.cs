using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RestartGameButton : MonoBehaviour {
	private Button _button;

	private void Awake() {
    _button = gameObject.GetComponent<Button>();
    _button.onClick.AddListener(RestartGame);
  }

  private void RestartGame() {
    Time.timeScale = 1;
    SceneManager.LoadScene("Scenes/LevelsScene");
  }
}
