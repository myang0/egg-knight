using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsDoneButton : MonoBehaviour
{
    private Button _button;
    public GameObject optionsPanel;

    private void Awake() {
        _button = gameObject.GetComponent<Button>();
        _button.onClick.AddListener(CloseOptions);
    }

    private void CloseOptions() {
        optionsPanel.SetActive(false);
    }
}
