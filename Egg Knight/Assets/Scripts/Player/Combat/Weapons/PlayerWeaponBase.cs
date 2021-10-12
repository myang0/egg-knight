using UnityEngine;

public abstract class PlayerWeaponBase : MonoBehaviour {
  protected Animator _anim;
  protected PolygonCollider2D _pCol;

  [SerializeField] protected float _damageAmount;
  [SerializeField] protected DamageType _damageType;

  protected void Awake() {
    _anim = gameObject.GetComponent<Animator>();
    _pCol = gameObject.GetComponent<PolygonCollider2D>();
  }

  protected virtual void FixedUpdate() {
    Transform player = GameObject.Find("Player").transform;

    transform.position = player.position;
  }

  protected virtual void OnTriggerEnter2D(Collider2D collider) {
    EnemyHealth enemyHealth = collider.gameObject.GetComponent<EnemyHealth>();
    
    if (enemyHealth != null) {
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