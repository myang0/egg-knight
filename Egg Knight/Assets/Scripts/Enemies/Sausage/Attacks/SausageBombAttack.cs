using System.Collections;
using UnityEngine;

public class SausageBombAttack : MonoBehaviour {
  [SerializeField] private float _attackDuration;

  [SerializeField] private int _bombsPerThrow;

  [SerializeField] private GameObject _bombObject;

  private Animator _anim;

  private Transform _playerTransform;

  private void Awake() {
    _anim = GetComponent<Animator>();
  }

  public void StartAttack() {
    StartCoroutine(BombDuration());
  }

  private IEnumerator BombDuration() {
    yield return new WaitForSeconds(_attackDuration);

    _anim.SetBool("IsBombing", false);
  }

  private void Bomb() {
    for (int i = 0; i < _bombsPerThrow; i++) {
      Instantiate(_bombObject, transform.position, Quaternion.identity);
    }
  }
}
