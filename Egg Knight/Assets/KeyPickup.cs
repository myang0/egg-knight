using System;
using System.Collections;
using System.Collections.Generic;
using Stage;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    private void Start() {
        var position = transform.position;
        transform.position = new Vector3(position.x, position.y, ZcoordinateConsts.Object);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            StageManager stage = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>().GetCurrentStage();
            if (stage.keyStatus == StageManager.KeyStageStatus.KeyNotFound)
                stage.keyStatus = StageManager.KeyStageStatus.KeyFound;
            Destroy(gameObject);
        }
    }
}
