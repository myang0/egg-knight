using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerDeath : MonoBehaviour {
    public GameObject deathScreen;
    public CinemachineVirtualCamera vcam;
    public bool isActive;
    public static event EventHandler OnDeathScreenStart;

    public void Awake() {
        PlayerHealth.OnGameOver += StartDeathSequence;
    }

    private void StartDeathSequence(object sender, EventArgs e) {
        var hitbox = gameObject.GetComponent<CapsuleCollider2D>();
        var footprint = gameObject.GetComponentInChildren<BoxCollider2D>();
        hitbox.enabled = false;
        footprint.enabled = false;
        StartCoroutine(ZoomIn());
        StartCoroutine(DeathRotation());
        PlayerHealth.OnGameOver -= StartDeathSequence;
    }
    
    private IEnumerator ZoomIn() {
        float timeDest = 3.4f;
        float timeCurr = 0;
        bool camTransition = true;
        float initialSize = vcam.m_Lens.OrthographicSize;
        while (camTransition) {
            timeCurr += Time.deltaTime / timeDest;
            vcam.m_Lens.OrthographicSize = Mathf.Lerp(initialSize, 1, timeCurr);
            if (vcam.m_Lens.OrthographicSize <= 1) {
                vcam.m_Lens.OrthographicSize = 1;
                camTransition = false;
            }
            yield return new WaitForFixedUpdate();
        }
        deathScreen.SetActive(true);
        OnDeathScreenStart?.Invoke(this, EventArgs.Empty);
    }

    private IEnumerator DeathRotation() {
        float timeDest = 3.5f;
        float timeCurr = 0;
        bool isRotating = true;
        var targetRotation = Quaternion.Euler(0, 0, 90);
        var initialRotation = transform.rotation;
        while (isRotating) {
            timeCurr += Time.deltaTime / timeDest;
            transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, timeCurr);
            if (transform.rotation.z >= 90) isRotating = false;
            yield return new WaitForFixedUpdate();
        }
    }
}
