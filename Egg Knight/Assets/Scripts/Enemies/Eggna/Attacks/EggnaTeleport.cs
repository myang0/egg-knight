using System.Collections;
using UnityEngine;

public class EggnaTeleport : MonoBehaviour {
  [SerializeField] private float _invisiblilityTime;

  private Animator _anim;
  private SpriteRenderer _sr;
  private Collider2D _collider;

  private Transform _playerTransform;

  [SerializeField] private LayerMask _playerLayer;

  [SerializeField] private float _attackRange;
  [SerializeField] private float _attackDamage;

  private void Awake() {
    _anim = GetComponent<Animator>();
    _sr = GetComponent<SpriteRenderer>();
    _collider = GetComponent<Collider2D>();

    _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
  }

  public void Disappear() {
    _sr.enabled = false;
    _collider.enabled = false;

    StartCoroutine(Invisibility());
  }
  
  private IEnumerator Invisibility() {
    yield return new WaitForSeconds(_invisiblilityTime);
  }

  public void Reappear() {
    transform.position = _playerTransform.position;
    
    _sr.enabled = true;
  }

  public void LandingAttack() {
    _collider.enabled = true;

    Collider2D[] playersInRange = Physics2D.OverlapCircleAll(transform.position, _attackRange, _playerLayer);
    PlayerHealth pHealth = playersInRange[0]?.gameObject?.GetComponent<PlayerHealth>();

    if (pHealth != null) {
      pHealth.Damage(_attackDamage);
    }
  }

  private void OnDrawGizmosSelected() {
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, _attackRange);
  }
}
