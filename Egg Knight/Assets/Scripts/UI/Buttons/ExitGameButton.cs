using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ExitGameButton : MonoBehaviour {
	private Button _button;

	private void Awake() {
    _button = gameObject.GetComponent<Button>();
    _button.onClick.AddListener(ExitGame);
  }

  private void ExitGame() {
    Application.Quit();
  }
}
