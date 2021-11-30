using System.Collections;
using UnityEngine;

public class EggnaSpin : MonoBehaviour {
  [SerializeField] private GameObject _daggerObject;

  [SerializeField] private int _numBursts;
  [SerializeField] private float _minTimeBetweenBursts = 0.25f;
  [SerializeField] private float _maxTimeBetweenBursts = 1;
  private float _timeBetweenBursts;

  [SerializeField] private int _daggersPerBurst;
  [SerializeField] private float _timeBetweenThrows;

  [SerializeField] private float _maxSpread;

  private Animator _anim;
  private EggnaHealth _eHealth;
  private EnemyBehaviour _eBehaviour;

  private Transform _playerTransform;

  private void Awake() {
    _timeBetweenBursts = _maxTimeBetweenBursts;

    _eHealth = GetComponent<EggnaHealth>();
    _anim = GetComponent<Animator>();
    _eBehaviour = GetComponent<EnemyBehaviour>();

    _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
  }

  public void StartAttack() {
    if (_daggerObject == null) {
      return;
    }

    _timeBetweenBursts = _minTimeBetweenBursts + (_eHealth.CurrentHealthPercentage() * (_maxTimeBetweenBursts - _minTimeBetweenBursts));

    StartCoroutine(SpinAttack());
  }

  private IEnumerator SpinAttack() {
    for (int i = 0; i < _numBursts; i++) {
      yield return new WaitForSeconds(_timeBetweenBursts);

      for (int j = 0; j < _daggersPerBurst; j++) {
        ThrowDagger();

        yield return new WaitForSeconds(_timeBetweenThrows);
      }
    }

    _anim.SetBool("IsSpinning", false);
  }

  private void ThrowDagger() {
    Vector2 vectorToPlayer = VectorHelper.GetVectorToPoint(transform.position, _playerTransform.position);
    Vector2 direction = Quaternion.Euler(0, 0, Random.Range(-_maxSpread, _maxSpread)) * vectorToPlayer;

    float angle = Vector2.SignedAngle(Vector2.up, direction);

    Dagger dagger = Instantiate(_daggerObject, transform.position, Quaternion.identity).GetComponent<Dagger>();

    ProjectileHelper.Refrigerate(_eBehaviour.PlayerInventory, dagger);
    dagger.SetDirection(direction, angle);
  }
}
