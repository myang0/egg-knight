using UnityEngine;

public class EggnaSlashSwoosh : BaseEnemyWeapon {
  [SerializeField] private float _attackRange;
  [SerializeField] private LayerMask _playerLayer;

  [SerializeField] private Transform _attackPoint;

  [SerializeField] private AudioClip _clip;

  public void SetRotation(float angle) {
    transform.eulerAngles = new Vector3(0, 0, angle);
  }

  public override void EnableHitbox() {
    Sound();

    Collider2D[] playersInRange = Physics2D.OverlapCircleAll(_attackPoint.position, _attackRange, _playerLayer);

    if (playersInRange.Length > 0) {
      DamagePlayer(playersInRange[0]);
    }
  }

  public void Sound() {
    SingleTimeSound sound = Instantiate(_singleTimeSound, transform.position, Quaternion.identity)
      .GetComponent<SingleTimeSound>();

    sound.ScaleVolume(2.0f);
    sound.LoadClipAndPlay(_clip);
  }

  public override void OnAnimationEnd() {
    Destroy(gameObject);
  }

  protected override void OnDrawGizmosSelected() {
    Gizmos.color = Color.red;

    if (_attackPoint != null) {
      Gizmos.DrawWireSphere(_attackPoint.position, _attackRange);
    }
  }
}
