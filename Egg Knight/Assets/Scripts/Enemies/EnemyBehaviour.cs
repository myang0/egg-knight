using System;
using System.Collections;
using Stage;
using UnityEngine;
using UnityEngine.Assertions;
using Pathfinding;

public abstract class EnemyBehaviour : MonoBehaviour {
  protected Rigidbody2D _rb;

  [SerializeField] protected float _maxSpeed;
  protected float _currentSpeed;

  [SerializeField] protected float _yolkedDuration;

  protected Health Health;
  
  private Transform _playerTransform;
  private float nextWaypointDistance = 2f;
  private Path _path;
  private int _currentWaypoint;
  private bool _reachedEndOfPath;
  private Seeker _seeker;
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
    
    _seeker = GetComponent<Seeker>();
    _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    InvokeRepeating(nameof(UpdatePath), 0f, 0.5f);
  }
  
  void UpdatePath() {
    if (_seeker.IsDone()) _seeker.StartPath(_rb.position, _playerTransform.position, OnPathComplete);
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

  private void OnPathComplete(Path p) {
    if (p.error) return;
    _path = p;
    _currentWaypoint = 0;
  }

  protected void MoveToPlayer() {
    if (_path == null) return;
    _reachedEndOfPath = _currentWaypoint >= _path.vectorPath.Count;
    

    Vector2 direction = ((Vector2)_path.vectorPath[_currentWaypoint] - _rb.position).normalized;
    Vector2 movementForce = direction * _currentSpeed;
    // Vector2 movementForce = direction * _currentSpeed * Time.deltaTime;
    
    _rb.velocity = movementForce;
    // _rb.AddForce(movementForce);

    float distanceToNextWaypoint = Vector2.Distance(_rb.position, _path.vectorPath[_currentWaypoint]);
    if (distanceToNextWaypoint < nextWaypointDistance) {
      _currentWaypoint++;
    }
  }

  private void OnCollisionEnter2D(Collision2D other) {
    if (other.collider.gameObject.layer == LayerMask.NameToLayer("Obstacle")) {
      Physics2D.IgnoreCollision(other.collider, GetComponent<Collider2D>());
    }
  }
}