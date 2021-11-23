using System.Collections;
using UnityEngine;

public class EggnaTeleport : MonoBehaviour {
  [SerializeField] private float _minInvisibilityTime;
  [SerializeField] private float _maxInvisibilityTime;
  private float _invisiblilityTime;

  [SerializeField] private GameObject _smokeParticles;

  [SerializeField] private Transform _reappearPoint;

  private Animator _anim;
  private SpriteRenderer _sr;
  private Collider2D _collider;

  private Transform _playerTransform;

  [SerializeField] private GameObject _swooshObject;

  private void Awake() {
    _invisiblilityTime = Random.Range(_minInvisibilityTime, _maxInvisibilityTime);

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
    Instantiate(_smokeParticles, transform.position, Quaternion.identity);

    yield return new WaitForSeconds(_invisiblilityTime);

    _invisiblilityTime = Random.Range(_minInvisibilityTime, _maxInvisibilityTime);

    _anim.SetBool("IsReappearing", true);
  }

  public void Reappear() {
    transform.position = _playerTransform.position;
    
    Instantiate(_smokeParticles, _reappearPoint.position, Quaternion.identity);
    
    _sr.enabled = true;
  }

  public void LandingAttack() {
    _collider.enabled = true;

    if (_swooshObject != null) {
      GameObject swooshObject = Instantiate(_swooshObject, transform.position, Quaternion.identity);
      swooshObject.GetComponent<EggnaSwoosh>().SetRotation(180); 
    }
  }
}
