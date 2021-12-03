using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerDeath : MonoBehaviour {
    public GameObject deathScreen;
    public CinemachineVirtualCamera vcam;
    public bool isReviving;
    public bool isDeathRotating;
    public bool camTransition;
    public bool isReviveRotating;
    public static event EventHandler OnDeathScreenStart;
    public static event EventHandler OnReviveSequenceDone;

    private float initialOrthoSize;
    
    public void Awake() {
        PlayerHealth.OnGameOver += StartDeathSequence;
        PlayerHealth.OnReviveGameOver += SetReviving;
        PlayerHealth.OnReviveGameOver += StartDeathSequence;
    }

    private void StartDeathSequence(object sender, EventArgs e) {
        initialOrthoSize = vcam.m_Lens.OrthographicSize;
        gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
        gameObject.GetComponentInChildren<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<Animator>().SetBool("Moving", false);
        StartCoroutine(ZoomIn());
        StartCoroutine(DeathRotation());
    }

    private void SetReviving(object sender, EventArgs e) {
        isReviving = true;
    }

    private IEnumerator ZoomIn() {
        float timeDest = 3.4f;
        float timeCurr = 0;
        camTransition = true;
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

        if (!isReviving) {
            deathScreen.SetActive(true);
            OnDeathScreenStart?.Invoke(this, EventArgs.Empty);
        }
        else {
            isDeathRotating = false;
            StartCoroutine(ZoomOut());
            StartCoroutine(ReviveRotation());
            isReviving = false;
        }
    }

    private IEnumerator DeathRotation() {
        float timeDest = 3.5f;
        float timeCurr = 0;
        var targetRotation = Quaternion.Euler(0, 0, 90);
        var initialRotation = transform.rotation;
        isDeathRotating = true;
        while (isDeathRotating) {
            timeCurr += Time.deltaTime / timeDest;
            transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, timeCurr);
            if (transform.rotation.z == 90) isDeathRotating = false;
            yield return new WaitForFixedUpdate();
        }
    }
    
    private IEnumerator ZoomOut() {
        float timeDest = 0.5f;
        float timeCurr = 0;
        camTransition = true;
        float initialSize = vcam.m_Lens.OrthographicSize;
        while (camTransition) {
            timeCurr += Time.deltaTime / timeDest;
            vcam.m_Lens.OrthographicSize = Mathf.Lerp(initialSize, initialOrthoSize, timeCurr);
            if (vcam.m_Lens.OrthographicSize >= initialOrthoSize) {
                vcam.m_Lens.OrthographicSize = initialOrthoSize;
                camTransition = false;
            }
            yield return new WaitForFixedUpdate();
        }
        gameObject.GetComponent<CapsuleCollider2D>().enabled = true;
        gameObject.GetComponentInChildren<BoxCollider2D>().enabled = true;
        OnReviveSequenceDone?.Invoke(this, EventArgs.Empty);
    }

    private IEnumerator ReviveRotation() {
        float timeDest = 0.5f;
        float timeCurr = 0;
        var targetRotation = Quaternion.Euler(0, 0, 0);
        var initialRotation = transform.rotation;
        isReviveRotating = true;
        while (isReviveRotating) {
            timeCurr += Time.deltaTime / timeDest;
            transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, timeCurr);
            if (transform.rotation.z == Quaternion.identity.z) isReviveRotating = false;
            yield return new WaitForFixedUpdate();
        }
    }

    private void OnDestroy() {
        PlayerHealth.OnGameOver -= StartDeathSequence;
        PlayerHealth.OnReviveGameOver -= SetReviving;
        PlayerHealth.OnReviveGameOver -= StartDeathSequence;
    }
}
