using System;
using System.Collections;
using System.Collections.Generic;
using Stage;
using UnityEngine;
using UnityEngine.Assertions;
using Pathfinding;

public abstract class EnemyBehaviour : MonoBehaviour {
  protected Rigidbody2D _rb;

  [SerializeField] protected float _maxSpeed;
  protected float _currentSpeed;

  protected Health Health;

  public event EventHandler OnYolked;
  public event EventHandler OnFrosted;
  public event EventHandler OnIgnited;
  public event EventHandler OnElectrocuted;
  
  public float maxDistanceToAttack;
  public float minDistanceToAttack;
  public float attackCooldownMax;
  public bool isAttackOffCooldown;
  public float alertRange = 4f;

  private Transform _playerTransform;
  private float nextWaypointDistance = 2f;
  private Path _path;
  private int _currentWaypoint;
  private bool _reachedEndOfPath;
  protected bool isWallCollisionOn;
  private Seeker _seeker;
  protected virtual void Awake() {
    Assert.IsNotNull(Health);
    _currentSpeed = _maxSpeed;
    
    _rb = gameObject.GetComponent<Rigidbody2D>();
    isAttackOffCooldown = true;
    
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
    List<StatusCondition> statuses = e.statuses;

    foreach (StatusCondition s in statuses) {
      HandleStatus(s);
    }
  }

  private void HandleStatus(StatusCondition status) {
    switch (status) {
      case StatusCondition.Yolked: {
        OnYolked?.Invoke(this, EventArgs.Empty);
        StartCoroutine(Yolked());
        break;
      }
      case StatusCondition.Ignited: {
        OnIgnited?.Invoke(this, EventArgs.Empty);
        break;
      }
      case StatusCondition.Frosted: {
        OnFrosted?.Invoke(this, EventArgs.Empty);
        StartCoroutine(Frosted());
        break;
      }
      case StatusCondition.Electrocuted: {
        OnElectrocuted?.Invoke(this, EventArgs.Empty);
        break;
      }
      default: {
        Debug.Log("Unknown status condition");
        break;
      }
    }
  }

  protected virtual IEnumerator Yolked() {
    _currentSpeed = _currentSpeed * StatusConfig.YolkedSpeedModifier;

    yield return new WaitForSeconds(StatusConfig.YolkedDuration);

    _currentSpeed = _currentSpeed / StatusConfig.YolkedSpeedModifier;
  }

  protected virtual IEnumerator Frosted() {
    _currentSpeed = _currentSpeed * StatusConfig.FrostSpeedMultiplier;

    yield return new WaitForSeconds(StatusConfig.FrostDuration);

    _currentSpeed = _currentSpeed / StatusConfig.FrostSpeedMultiplier;
  }

  private void OnPathComplete(Path p) {
    if (p.error) return;
    _path = p;
    _currentWaypoint = 0;
  }

  public void MoveToPlayer() {
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
  
  public void Flee() {
    Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
    Vector2 vectorFromPlayer = VectorHelper.GetVectorToPoint(playerPos, transform.position);

    _rb.velocity = vectorFromPlayer * _currentSpeed;
  }


  public float GetDistanceToPlayer() {
    return Vector2.Distance(transform.position, _playerTransform.position);
  }

  public bool GetIsAttackOffCooldown() {
    return isAttackOffCooldown;
  }

  public virtual bool GetIsAttackReady() {
    return GetDistanceToPlayer() < maxDistanceToAttack && isAttackOffCooldown;
  }

  public bool GetIsInAlertRange() {
    return GetDistanceToPlayer() < alertRange;
  }
  

  private void OnCollisionEnter2D(Collision2D other) {
    if (!isWallCollisionOn) {
      if (other.collider.gameObject.layer == LayerMask.NameToLayer("Obstacle")) {
        Physics2D.IgnoreCollision(other.collider, GetComponent<Collider2D>());
      }
    }
  }

  public void Attack() {
    StartCoroutine(AttackPlayer());
  }

  protected abstract IEnumerator AttackPlayer();
}
