using System.Collections;
using UnityEngine;

public class SausageBombAttack : MonoBehaviour {
  [SerializeField] private float _attackDuration;

  [SerializeField] private int _bombsPerThrow;

  [SerializeField] private GameObject _bombObject;

  private Animator _anim;
  private SoundPlayer _soundPlayer;

  [SerializeField] private AudioClip _useClip;

  private Transform _playerTransform;

  private void Awake() {
    _anim = GetComponent<Animator>();
    _soundPlayer = GetComponent<SoundPlayer>();
  }

  public void StartAttack() {
    StartCoroutine(BombDuration());
  }

  private IEnumerator BombDuration() {
    yield return new WaitForSeconds(_attackDuration);

    _anim.SetBool("IsBombing", false);
  }

  private void Bomb() {
    _soundPlayer.PlayClip(_useClip, volumeScaling: 2f);

    for (int i = 0; i < _bombsPerThrow; i++) {
      Instantiate(_bombObject, transform.position, Quaternion.identity);
    }
  }
}
