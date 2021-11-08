using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class TutorialRoom : MonoBehaviour {
    public List<DeadTreeBehavior> deadTrees = new List<DeadTreeBehavior>();
    public Transform EggithaTransform;
    public Transform EntitySpawnPosTransform;
    public bool isPlayerInside;
    public bool isRoomCleared;
    public EventHandler OnRoomEnter;
    public EventHandler OnRoomExit;

    IEnumerator DelayEnterInvocation() {
        yield return new WaitForSeconds(0.5f);
        OnRoomEnter?.Invoke(this, EventArgs.Empty);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            isPlayerInside = true;
            StartCoroutine(DelayEnterInvocation());
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            isPlayerInside = false;
            OnRoomExit?.Invoke(this, EventArgs.Empty);
        }
    }
}
