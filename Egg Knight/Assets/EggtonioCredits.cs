using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggtonioCredits : MonoBehaviour {
    public SpriteRenderer sprite;
    public Transform hideSpot;
    public Transform standSpot;
    public bool isFastWobble;

    private Animator _animator;
    public static event EventHandler OnOutroEnd;
    public static event EventHandler OnOutroStart;

    private void Awake() {
        _animator = GetComponent<Animator>();
    }

    public void InvokeOnOutroStart() {
        OnOutroStart?.Invoke(this, EventArgs.Empty);
    }
    
    public void InvokeOnOutroEnd() {
        OnOutroEnd?.Invoke(this, EventArgs.Empty);
    }

    public void StartWobble() {
        StartCoroutine(Wobble());
    }

    public void StartStopWobble() {
        StopCoroutine(StopWobble());
    }

    public void StartMoveToStandSpot() {
        StartCoroutine(MoveToStandSpot());
    }
    
    public void StartMoveToHideSpot() {
        StartCoroutine(MoveToHideSpot());
    }

    public void FireTrigger() {
        _animator.SetTrigger("NextState");
        // _animator.ResetTrigger("NextState");
    }


    private IEnumerator MoveToStandSpot() {
        float timeDest = 3f;
        float timeCurr = 0f;

        while (transform.position != standSpot.position) {
            timeCurr += Time.deltaTime / timeDest;
            transform.position = Vector2.Lerp(hideSpot.position, standSpot.position, timeCurr);
            yield return null;
        }
    }

    private IEnumerator MoveToHideSpot() {
        sprite.flipX = true;
        float timeDest = 3f;
        float timeCurr = 0f;

        while (transform.position != hideSpot.position) {
            timeCurr += Time.deltaTime / timeDest;
            transform.position = Vector2.Lerp(standSpot.position, hideSpot.position, timeCurr);
            yield return null;
        }
    }

    private IEnumerator StopWobble() {
        while (transform.rotation.z != 0) yield return null;
        StopCoroutine(Wobble());
    }

    private IEnumerator Wobble() {
        float timeDestFast = 0.3f;
        float timeDestSlow = 1f;
        
        float angleFast = 10f;
        float angleSlow = 5f;
        
        float timeCurr = 0f;
        bool rotateForward = true;
        
        var forwardRotationFast = Quaternion.Euler(0, 0, angleFast);
        var backwardRotationFast = Quaternion.Euler(0, 0, -angleFast);
        
        var forwardRotationSlow = Quaternion.Euler(0, 0, angleSlow);
        var backwardRotationSlow = Quaternion.Euler(0, 0, -angleSlow);
        
        var initialRotation = transform.rotation;
        
        while (true) {
            if (isFastWobble) timeCurr += Time.deltaTime / timeDestFast;
            else timeCurr += Time.deltaTime / timeDestSlow;
            
            if (rotateForward) {
                if (isFastWobble) {transform.rotation = Quaternion.Lerp(initialRotation, forwardRotationFast, timeCurr);}
                else transform.rotation = Quaternion.Lerp(initialRotation, forwardRotationSlow, timeCurr);
                if (isFastWobble && Math.Abs(transform.rotation.z - forwardRotationFast.z) < 0.01f ||
                    !isFastWobble && Math.Abs(transform.rotation.z - forwardRotationSlow.z) < 0.01f) {
                    rotateForward = false;
                    initialRotation = transform.rotation;
                    timeCurr = 0;
                }
            }
            else {
                if (isFastWobble) transform.rotation = Quaternion.Lerp(initialRotation, backwardRotationFast, timeCurr);
                else transform.rotation = Quaternion.Lerp(initialRotation, backwardRotationSlow, timeCurr);
                if (isFastWobble && Math.Abs(transform.rotation.z - backwardRotationFast.z) < 0.01f ||
                    !isFastWobble && Math.Abs(transform.rotation.z - backwardRotationSlow.z) < 0.01f) {
                    rotateForward = true;
                    initialRotation = transform.rotation;
                    timeCurr = 0;
                }
            }
            yield return new WaitForFixedUpdate();
        }
    }
}
