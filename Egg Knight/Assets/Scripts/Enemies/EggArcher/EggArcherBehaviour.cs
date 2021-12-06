using System;
using System.Collections;
using UnityEngine;

public class EggArcherBehaviour : EnemyBehaviour {
  private Animator _anim;
  private Collider2D _collider;

  [SerializeField] private EggArcherBow _bow;
  public EggArcherBow Bow {
    get {
      return _bow;
    }
  }

  protected override void Awake() {
    _anim = GetComponent<Animator>();
    _collider = GetComponent<Collider2D>();

    EggArcherHealth eggArcherHealth = gameObject.GetComponent<EggArcherHealth>();
    eggArcherHealth.OnEggArcherStatusDamage += HandleStatusDamage;

    EnemyBehaviour enemyBehaviour = gameObject.GetComponent<EnemyBehaviour>();
    enemyBehaviour.OnElectrocuted += HandleElectrocuted;

    maxDistanceToAttack = 5f;
    attackCooldownMax = 2.5f;

    Health = eggArcherHealth;
    base.Awake();
  }

  protected override void Update() {
    if (isDead) return;

    SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

    if (_anim.GetBool("IsFleeing") && isTurningEnabled) {
      spriteRenderer.flipX = rb.velocity.x <= 0;
    } else if (!isWandering && isTurningEnabled) {
      spriteRenderer.flipX = transform.position.x - _playerTransform.position.x > 0;
    }
  }

  public bool HasClearShot() {
    Vector2 vectorToPlayer = VectorHelper.GetVectorToPoint(transform.position, _playerTransform.position);
    RaycastHit2D firstHit = Physics2D.Raycast(
      transform.position,
      vectorToPlayer,
      (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("Obstacle"))
    );

    return firstHit.transform.gameObject.CompareTag("Player");
  }

  public void LimitFleeDuration() {
    StartCoroutine(LimitFleeDurationCoroutine());
  }

  private IEnumerator LimitFleeDurationCoroutine() {
    yield return new WaitForSeconds(1f);
    if (_anim.GetBool("IsFleeing")) {
      _anim.SetBool("IsFleeing", false);
      Debug.Log("OK");
    } else     Debug.Log("OKYY");
  }

  protected override void OnCollisionEnter2D(Collision2D other) {
    if (_anim.GetBool("IsRolling") && other.collider.gameObject.layer == LayerMask.NameToLayer("PlayerFootprint")) {
      Physics2D.IgnoreCollision(other.collider, _collider);
    }
    
    if (_anim.GetBool("IsFleeing")) {
      if (other.collider.gameObject.layer == LayerMask.NameToLayer("Obstacle")) {
        if (_anim.GetBool("IsFleeing")) _anim.SetBool("IsFleeing", false);
      }
    }

    base.OnCollisionEnter2D(other);
  }

  private void HandleElectrocuted(object sender, EventArgs e) {
    StartCoroutine(Electrocute());
  }

  protected override IEnumerator AttackPlayer() {
    yield break;
  }
}
