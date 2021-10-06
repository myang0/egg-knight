using System;
using System.Collections;
using UnityEngine;

public abstract class EnemyBehaviour : MonoBehaviour {
  protected Rigidbody2D _rb;

  [SerializeField] protected float _maxSpeed;
  protected float _currentSpeed;

  [SerializeField] protected float _yolkedDuration;

  protected virtual void Awake() {
    _currentSpeed = _maxSpeed;

    _rb = gameObject.GetComponent<Rigidbody2D>();
  }

  protected virtual void HandleStatusDamage(object sender, EnemyStatusEventArgs e) {
    StatusCondition status = e.status;

    switch (status) {
      case StatusCondition.Yolked: {
        StartCoroutine(Yolked());
        break;
      }
      case StatusCondition.Ignited: {
        Debug.Log("Ignited!");
        break;
      }
      case StatusCondition.Scrambled: {
        Debug.Log("Scrambled!");
        break;
      }
      default: {
        Debug.Log("Unknown status condition");
        break;
      }
    }
  }

  protected virtual IEnumerator Yolked() {
    _currentSpeed = _maxSpeed * 0.5f;

    yield return new WaitForSeconds(_yolkedDuration);

    _currentSpeed = _maxSpeed;
  }
}