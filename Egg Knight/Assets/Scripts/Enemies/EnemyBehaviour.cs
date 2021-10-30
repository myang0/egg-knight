using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Stage;
using UnityEngine;
using UnityEngine.Assertions;
using Pathfinding;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public abstract class EnemyBehaviour : MonoBehaviour {
  public Rigidbody2D rb;

  [SerializeField] protected float _maxSpeed;
  protected float _currentSpeed;

  protected Health Health;

  public event EventHandler OnYolked;
  public event EventHandler OnFrosted;
  public event EventHandler OnIgnited;
  public event EventHandler OnElectrocuted;
  public event EventHandler OnBleed;

  public float maxDistanceToAttack;
  public float minDistanceToAttack;
  public float attackCooldownMax;
  public bool isAttackOffCooldown;
  public bool isInAttackAnimation;
  public bool isStunned;
  public float alertRange;

  public bool isWandering;
  private Vector2 _wanderDestination;

  private Transform _playerTransform;
  public bool isWallCollisionOn;
  private EnemyMovement _eMovement;

  [SerializeField] private Animator alertAnimator;
  public bool isDead;

  protected virtual void Awake() {
    Assert.IsNotNull(Health);
    _currentSpeed = _maxSpeed;
    alertRange = 6f;

    rb = gameObject.GetComponent<Rigidbody2D>();
    isAttackOffCooldown = true;
    _eMovement = gameObject.GetComponent<EnemyMovement>();

    Health.OnPreDeath += (sender, args) => {
      isDead = true;
      GetComponent<Collider2D>().enabled = false;
      StartCoroutine(FadeOutDeath());
    };

    Health.OnDeath += (sender, eventArgs) => {
      FindObjectOfType<CoinDrop>().DropCoin(transform.position);
      GameObject.FindGameObjectWithTag("LevelManager")
        .GetComponent<LevelManager>()
        .GetCurrentStage()
        .RemoveEnemy(this);
    };

    _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    InvokeRepeating(nameof(InterruptWander), 0f, 2f);
  }

  protected void Update() {
    if (isDead) return;
    if (!isWandering) {
      SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
      spriteRenderer.flipX = transform.position.x - _playerTransform.position.x > 0;
    }
  }

  private IEnumerator FadeOutDeath() {
    Quaternion newRotation = Quaternion.Euler(0, 0, 90);
    SpriteRenderer sr = GetComponent<SpriteRenderer>();
    while (sr.color.a > 0) {
      transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 7.5f);
      var color = sr.color;
      float newAlpha = color.a -= 0.001f;
      sr.color = new Color(color.r, color.g, color.b, newAlpha);
      yield return null;
    }
  }

  private void InterruptWander() {
    if (isWandering) {
      isWandering = false;
    }
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
      case StatusCondition.Bleeding: {
        OnBleed?.Invoke(this, EventArgs.Empty);
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
  
  protected IEnumerator Electrocute() {
    isStunned = true;

    StopMoving();

    yield return new WaitForSeconds(StatusConfig.ElectrocuteStunDuration);
    isStunned = false;
  }

  public void Move() {
    _eMovement.MoveToPlayer(_currentSpeed);
  }

  public void Flee() {
    _eMovement.Flee(_currentSpeed);
  }

  public void StopMoving() {
    rb.velocity = Vector2.zero;
  }

  public void Wander() {
    if (isWandering) return;
    StartCoroutine(WanderIEnumerator());
  }

  private IEnumerator WanderIEnumerator() {
    isWandering = true;
    var position = transform.position;
    float newLocationX = position.x + Random.Range(-2f, 2f);
    float newLocationY = position.y + Random.Range(-2f, 2f);
    _wanderDestination = new Vector2(newLocationX, newLocationY);

    while (Vector2.Distance(transform.position, _wanderDestination) > 0.2 && !isStunned) {
      if (isStunned || !isWandering) yield break;
      transform.position = Vector2.MoveTowards(transform.position, _wanderDestination, _maxSpeed/4*Time.deltaTime);
      
      SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
      spriteRenderer.flipX = transform.position.x - _wanderDestination.x > 0;
      yield return null;
    }

    isWandering = false;
  }

  public float GetDistanceToPlayer() {
    return Vector2.Distance(transform.position, _playerTransform.position);
  }
  
  public virtual bool GetIsAttackReady() {
    return GetDistanceToPlayer() < maxDistanceToAttack && isAttackOffCooldown && !isInAttackAnimation;
  }

  public bool GetIsInAlertRange() {
    return GetDistanceToPlayer() < alertRange;
  }

  public void SetAlertTrigger() {
    alertAnimator.Play("Active",  0, 0f);
  }

  private void OnCollisionEnter2D(Collision2D other) {
    if (!isWallCollisionOn && !isWandering && other.collider.gameObject.layer == LayerMask.NameToLayer("Obstacle")) {
      Physics2D.IgnoreCollision(other.collider, GetComponent<Collider2D>());
    }

    // if (other.collider.CompareTag("Enemy")) {
    //   if (other.collider.GetComponent<EnemyBehaviour>().isDead) {
    //     Physics2D.IgnoreCollision(other.collider, GetComponent<Collider2D>());
    //   }
    // }
  }

  public virtual void Attack() {
    StartCoroutine(AttackPlayer());
  }

  protected abstract IEnumerator AttackPlayer();
}
