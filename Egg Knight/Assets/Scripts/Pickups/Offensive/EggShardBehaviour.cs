using System.Collections;
using UnityEngine;

public class EggShardBehaviour : MonoBehaviour {
  [SerializeField] private float _contactDamage;

  [SerializeField] private float _timeToDespawn;

  private void Awake() {
    PlayerWeapons pWeapons = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerWeapons>();
    _contactDamage *= pWeapons.GetDamageMultiplier();
    StartCoroutine(Despawn());
  }

  private void OnTriggerEnter2D(Collider2D col) {
    EnemyHealth enemyHealth = col.gameObject.GetComponent<EnemyHealth>();
    
    if (enemyHealth != null) {
      enemyHealth.Damage(_contactDamage);

      Destroy(gameObject);
    }
  }

  private IEnumerator Despawn() {
    yield return new WaitForSeconds(_timeToDespawn);

    Destroy(gameObject);
  }
}
