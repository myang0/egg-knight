using UnityEngine;

public class EggnaThrow : MonoBehaviour {
  [SerializeField] private GameObject _daggerObject;

  [SerializeField] private int _numDaggers;
  [SerializeField] private float _angleBetweenDaggers;

  private EnemyBehaviour _eBehaviour;

  private Transform _playerTransform;

  private void Awake() {
    _eBehaviour = GetComponent<EnemyBehaviour>();

    _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
  }

  public void ThrowDaggers() {
    if (_daggerObject == null) {
      return;
    }

    Vector2 vectorToPlayer = VectorHelper.GetVectorToPoint(transform.position, _playerTransform.position);

    int halfNumDaggers = _numDaggers / 2;
    for (int i = -halfNumDaggers; i <= halfNumDaggers; i++) {
      SpawnDagger(Quaternion.Euler(0, 0, i * _angleBetweenDaggers) * vectorToPlayer);
    }
  }

  private void SpawnDagger(Vector2 direction) {
    float angle = Vector2.SignedAngle(Vector2.up, direction);

    Dagger dagger = Instantiate(_daggerObject, transform.position, Quaternion.identity).GetComponent<Dagger>();

    ProjectileHelper.Refrigerate(_eBehaviour.PlayerInventory, dagger);
    dagger.SetDirection(direction, angle);
  }
}
