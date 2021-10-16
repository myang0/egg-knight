using System;
using System.Collections;
using Stage;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class EnemyBehaviour : MonoBehaviour {
  protected Rigidbody2D _rb;

  [SerializeField] protected float _maxSpeed;
  protected float _currentSpeed;

  [SerializeField] protected float _yolkedDuration;

  protected Health Health;
  protected virtual void Awake() {
    Assert.IsNotNull(Health);
    _currentSpeed = _maxSpeed;
    
    _rb = gameObject.GetComponent<Rigidbody2D>();
    
    Health.OnDeath += (sender, eventArgs) => {
      FindObjectOfType<CoinDrop>().DropCoin(transform.position);
      GameObject.FindGameObjectWithTag("LevelManager")
        .GetComponent<LevelManager>()
        .GetCurrentStage()
        .RemoveEnemy(this);
    };
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
      case StatusCondition.Frosted: {
        Debug.Log("Frosted!");
        break;
      }
      case StatusCondition.Electrocuted: {
        Debug.Log("Electrocuted!");
        break;
      }
      default: {
        Debug.Log("Unknown status condition");
        break;
      }
    }
  }

  protected virtual IEnumerator Yolked() {
    _currentSpeed = _currentSpeed * 0.5f;

    yield return new WaitForSeconds(_yolkedDuration);

    _currentSpeed = _currentSpeed * 2;
  }
}