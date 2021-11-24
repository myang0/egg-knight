using System.Collections;
using UnityEngine;

public class BaconAttack : MonoBehaviour {
  [SerializeField] private float _cooldownTime;

  [SerializeField] private GameObject _greaseOrbPrefab;

  private Animator _anim;

  private void Awake() {
    _anim = GetComponent<Animator>();
  }

  public void SpawnOrb() {
    if (_greaseOrbPrefab != null) {
      Instantiate(_greaseOrbPrefab, transform.position, Quaternion.identity);

      StartCoroutine(Cooldown());
    }
  }

  private IEnumerator Cooldown() {
    yield return new WaitForSeconds(_cooldownTime);

    _anim.SetBool("IsAttacking", false);
  }
}
