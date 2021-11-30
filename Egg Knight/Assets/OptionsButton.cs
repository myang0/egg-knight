using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OptionsButton : MonoBehaviour
{
    private Button _button;
    public GameObject optionsPanel;

    private void Awake() {
        _button = gameObject.GetComponent<Button>();
        _button.onClick.AddListener(OpenOptions);
    }

    private void OpenOptions() {
        optionsPanel.SetActive(true);
    }
}
