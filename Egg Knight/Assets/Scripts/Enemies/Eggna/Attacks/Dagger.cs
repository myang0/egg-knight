using UnityEngine;
using UnityEngine.Tilemaps;

public class Dagger : Projectile {
  [SerializeField] private AudioClip _clip;
  [SerializeField] private GameObject _singleTimeSound;

  protected override void Awake() {
    Instantiate(_singleTimeSound, transform.position, Quaternion.identity)
      .GetComponent<SingleTimeSound>()
      .LoadClipAndPlay(_clip);

    base.Awake();
  }

  protected override void Despawn() {
    Destroy(gameObject);
  }

  protected override void OnTriggerEnter2D(Collider2D collider) {
    PlayerHealth playerHealth = collider.gameObject.GetComponent<PlayerHealth>();
    
    if (playerHealth != null) {
      playerHealth.Damage(_damage);
    }

    if (collider.gameObject.layer == LayerMask.NameToLayer("Obstacle") || playerHealth != null) {
      StopCoroutine(DespawnTimer());
      Despawn();
    }
  }
}
