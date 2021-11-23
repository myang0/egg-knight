using System.Collections;
using UnityEngine;

public class EggnaSpin : MonoBehaviour {
  [SerializeField] private GameObject _daggerObject;

  [SerializeField] private int _numBursts;
  [SerializeField] private float _minTimeBetweenBursts;
  [SerializeField] private float _maxTimeBetweenBursts;
  private float _timeBetweenBursts;

  [SerializeField] private int _daggersPerBurst;
  [SerializeField] private float _timeBetweenThrows;

  [SerializeField] private float _maxSpread;

  private Animator _anim;
  private EggnaHealth _eHealth;

  private Transform _playerTransform;

  private void Awake() {
    _timeBetweenBursts = _minTimeBetweenBursts;

    _eHealth = GetComponent<EggnaHealth>();
    _anim = GetComponent<Animator>();

    _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
  }

  public void StartAttack() {
    if (_daggerObject == null) {
      return;
    }

    StartCoroutine(SpinAttack());
  }

  private IEnumerator SpinAttack() {
    _eHealth.isInvulnerable = true;

    for (int i = 0; i < _numBursts; i++) {
      yield return new WaitForSeconds(_timeBetweenBursts);

      for (int j = 0; j < _daggersPerBurst; j++) {
        ThrowDagger();

        yield return new WaitForSeconds(_timeBetweenThrows);
      }

      _timeBetweenBursts = Random.Range(_minTimeBetweenBursts, _maxTimeBetweenBursts);
    }

    _anim.SetBool("IsSpinning", false);

    _eHealth.isInvulnerable = false;
  }

  private void ThrowDagger() {
    Vector2 vectorToPlayer = VectorHelper.GetVectorToPoint(transform.position, _playerTransform.position);
    Vector2 direction = Quaternion.Euler(0, 0, Random.Range(-_maxSpread, _maxSpread)) * vectorToPlayer;

    float angle = Vector2.SignedAngle(Vector2.up, direction);

    GameObject daggerObject = Instantiate(_daggerObject, transform.position, Quaternion.identity);
    daggerObject.GetComponent<Dagger>().SetDirection(direction, angle);
  }
}
