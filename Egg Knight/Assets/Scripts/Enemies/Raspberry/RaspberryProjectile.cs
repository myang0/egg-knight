using UnityEngine;
using UnityEngine.Tilemaps;

public class RaspberryProjectile : Projectile {
  [SerializeField] private AudioClip _clip;
  [SerializeField] private SingleTimeSound _singleTimeSound;

  protected override void Awake() {
    SingleTimeSound sound = Instantiate(_singleTimeSound, transform.position, Quaternion.identity)
      .GetComponent<SingleTimeSound>();
    
    sound.ScaleVolume(2f);
    sound.LoadClipAndPlay(_clip);

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