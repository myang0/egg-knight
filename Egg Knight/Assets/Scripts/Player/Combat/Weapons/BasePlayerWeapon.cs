using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePlayerWeapon : MonoBehaviour {
  protected Animator _anim;
  protected SpriteRenderer _sr;
  protected PolygonCollider2D _pCol;

  protected List<StatusCondition> _weaponModifiers;

  [SerializeField] protected float _damageAmount;
  [SerializeField] protected DamageType _damageType;

  protected void Awake() {
    _anim = gameObject.GetComponent<Animator>();
    _sr = gameObject.GetComponent<SpriteRenderer>();
    _pCol = gameObject.GetComponent<PolygonCollider2D>();

    _weaponModifiers = new List<StatusCondition>();

    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    _sr.flipX = (mousePos.x < transform.position.x);
  }

  public void SetModifiers(List<StatusCondition> modifiers) {
    foreach (StatusCondition modifier in modifiers) {
      _weaponModifiers.Add(modifier);
    }
  }

  protected virtual void FixedUpdate() {
    Transform player = GameObject.Find("Player").transform;

    transform.position = player.position;
  }

  protected virtual void OnTriggerEnter2D(Collider2D collider) {
    EnemyHealth enemyHealth = collider.gameObject.GetComponent<EnemyHealth>();
    
    if (enemyHealth != null) {
      DamageEnemy(enemyHealth);
    }
  }

  protected virtual void DamageEnemy(EnemyHealth enemyHealth) {
    List<StatusCondition> statuses = new List<StatusCondition>();

    foreach (StatusCondition modifier in _weaponModifiers) {
      int randomNum = Random.Range(0, 100);
      
      if (randomNum < StatusConfig.StatusEffectChance && !statuses.Contains(modifier)) {
        statuses.Add(modifier);
      }
    }

    if (statuses.Any()) {
      enemyHealth.DamageWithStatusesAndType(_damageAmount, statuses, _damageType);
    } else {
      enemyHealth.DamageWithType(_damageAmount, _damageType);
    }
  }

  public virtual void EnableCollider() {
    _pCol.enabled = true;
  }

  public virtual void DisableCollider() {
    _pCol.enabled = false;
  }

  public virtual void OnAnimationEnd() {
    Destroy(gameObject);
  }
}
