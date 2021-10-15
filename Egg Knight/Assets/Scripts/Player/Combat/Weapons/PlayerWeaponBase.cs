using UnityEngine;

public abstract class PlayerWeaponBase : MonoBehaviour {
  protected Animator _anim;
  protected SpriteRenderer _sr;
  protected PolygonCollider2D _pCol;

  [SerializeField] protected float _damageAmount;
  [SerializeField] protected DamageType _damageType;

  protected void Awake() {
    _anim = gameObject.GetComponent<Animator>();
    _sr = gameObject.GetComponent<SpriteRenderer>();
    _pCol = gameObject.GetComponent<PolygonCollider2D>();

    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    _sr.flipX = (mousePos.x < transform.position.x);
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
