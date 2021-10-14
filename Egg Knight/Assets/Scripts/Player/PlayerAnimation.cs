using System;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {
  private SpriteRenderer _sr;

  private bool _iFramesActive = false;

  private bool _isRolling = false;

  private float _anglesPerFrame = 0.0f;

  private void Awake() {
    PlayerMovement.OnRollBegin += HandleRollBegin;
    PlayerMovement.OnRollEnd += HandleRollEnd;

    PlayerHealth.OnIFramesEnabled += (object sender, EventArgs e) => _iFramesActive = true;
    PlayerHealth.OnIFramesDisabled += (object sender, EventArgs e) => {
      _iFramesActive = false;
      _sr.color = new Color(1, 1, 1, 1);
    };

    _sr = gameObject.GetComponent<SpriteRenderer>();
  }

  private void HandleRollBegin(object sender, RollEventArgs e) {
    _isRolling = true;

    float framesPerSecond = 50;
    _anglesPerFrame = (360.0f / (framesPerSecond * e.duration)) * ((e.direction == Direction.Right) ? -1 : 1);
  }

  private void HandleRollEnd(object sender, EventArgs e) {
    transform.eulerAngles = Vector3.zero;
    _isRolling = false;
  }

  private void Update() {
    if (_iFramesActive) {
      float alpha = _sr.color.a;

      _sr.color = new Color(1, 1, 1, (alpha == 1) ? 0.25f : 1);
    }
  }

  private void FixedUpdate() {
    if (_isRolling) {
      transform.Rotate(0, 0, _anglesPerFrame);
    }
  }
}
