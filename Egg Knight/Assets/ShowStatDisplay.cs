using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowStatDisplay : MonoBehaviour {
    public GameObject statDisplay;

    public void TurnOnStatDisplay(bool on) {
        if (on) {
            statDisplay.SetActive(true);
            statDisplay.GetComponent<StatDisplayManager>().UpdateStats(this, EventArgs.Empty);
        } else statDisplay.SetActive(false);
    }
}
