using System.Collections;
using UnityEngine;

public class BaconAttack : MonoBehaviour {
  [SerializeField] private float _cooldownTime;

  [SerializeField] private GameObject _greaseOrbPrefab;

  private Animator _anim;
  private EnemyBehaviour _eBehaviour;

  private void Awake() {
    _anim = GetComponent<Animator>();
    _eBehaviour = GetComponent<EnemyBehaviour>();
  }

  public void SpawnOrb() {
    if (_greaseOrbPrefab != null) {
      GreaseOrb orb = Instantiate(_greaseOrbPrefab, transform.position, Quaternion.identity).GetComponent<GreaseOrb>();
      ProjectileHelper.Refrigerate(_eBehaviour.PlayerInventory, orb);

      StartCoroutine(Cooldown());
    }
  }

  private IEnumerator Cooldown() {
    yield return new WaitForSeconds(_cooldownTime);

    _anim.SetBool("IsAttacking", false);
  }
}
