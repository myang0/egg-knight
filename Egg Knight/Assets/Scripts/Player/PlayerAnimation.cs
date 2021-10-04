using System;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {
  private bool _isRolling = false;

  private float _anglesPerFrame = 0.0f;

  private void Awake() {
    PlayerMovement.OnRollBegin += HandleRollBegin;
    PlayerMovement.OnRollEnd += HandleRollEnd;
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

  private void FixedUpdate() {
    if (_isRolling) {
      transform.Rotate(0, 0, _anglesPerFrame);
    }
  }
}
