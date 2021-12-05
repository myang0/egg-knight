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
  private EnemyHealth _eHealth;
  private SoundPlayer _soundPlayer;

  private Transform _playerTransform;

  [SerializeField] private GameObject _swooshObject;

  [SerializeField] private AudioClip _clip;
  [SerializeField] private AudioClip _swooshClip;

  private void Awake() {
    _invisiblilityTime = _maxInvisibilityTime;

    _anim = GetComponent<Animator>();
    _sr = GetComponent<SpriteRenderer>();
    _collider = GetComponent<Collider2D>();
    _eHealth = GetComponent<EnemyHealth>();
    _soundPlayer = GetComponent<SoundPlayer>();

    _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
  }

  public void Disappear() {
    _soundPlayer.PlayClip(_clip);

    _sr.enabled = false;
    _collider.enabled = false;

    _invisiblilityTime = _minInvisibilityTime + (_eHealth.CurrentHealthPercentage() * (_maxInvisibilityTime - _minInvisibilityTime));

    StartCoroutine(Invisibility());
  }
  
  private IEnumerator Invisibility() {
    Instantiate(_smokeParticles, transform.position, Quaternion.identity);

    yield return new WaitForSeconds(_invisiblilityTime);

    _invisiblilityTime = Random.Range(_minInvisibilityTime, _maxInvisibilityTime);

    _anim.SetBool("IsReappearing", true);
  }

  public void Reappear() {
    _soundPlayer.PlayClip(_clip);

    transform.position = _playerTransform.position;
    
    Instantiate(_smokeParticles, _reappearPoint.position, Quaternion.identity);
    
    _sr.enabled = true;
  }

  public void PlaySwoosh() {
    _soundPlayer.PlayClip(_swooshClip);
  }

  public void LandingAttack() {
    _collider.enabled = true;

    if (_swooshObject != null) {
      GameObject swooshObject = Instantiate(_swooshObject, transform.position, Quaternion.identity);
      swooshObject.GetComponent<EggnaSwoosh>().SetRotation(180); 
    }
  }
}
