using UnityEngine;

public abstract class BaseEnemyWeapon : MonoBehaviour {
  protected SpriteRenderer _sr;

  protected GameObject _playerObject;

  [SerializeField] protected float _damageAmount;

  [SerializeField] protected GameObject _singleTimeSound;

  protected virtual void Awake() {
    _sr = gameObject.GetComponent<SpriteRenderer>();

    _playerObject = GameObject.FindGameObjectWithTag("Player");

    Vector3 playerPos = _playerObject.transform.position;
    _sr.flipX = (playerPos.x < transform.position.x);
  }

  protected void DamagePlayer(Collider2D player) {
    PlayerHealth playerHealth = player.gameObject.GetComponent<PlayerHealth>();

    if (playerHealth != null) {
      playerHealth.Damage(_damageAmount);
    }
  }

  protected virtual void PlaySound(AudioClip clip) {
    Instantiate(_singleTimeSound, transform.position, Quaternion.identity)
      .GetComponent<SingleTimeSound>()
      .LoadClipAndPlay(clip);
  }

  public abstract void EnableHitbox();

  public abstract void OnAnimationEnd();

  protected abstract void OnDrawGizmosSelected();
}
