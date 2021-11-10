using System.Collections;
using UnityEngine;

public class PeaSpawner : MonoBehaviour {
  [SerializeField] private GameObject _peaObject;

  [SerializeField] private int _peasToBeSpawned;
  [SerializeField] private float _timeBetweenSpawns;

  private void Awake() {
    if (_peaObject != null) {
      StartCoroutine(SpawnPeas());
    } else {
      Destroy(gameObject);
    }
  }

  private IEnumerator SpawnPeas() {
    for (int i = 0; i < _peasToBeSpawned; i++) {
      Instantiate(_peaObject, transform.position, Quaternion.identity);
      yield return new WaitForSeconds(_timeBetweenSpawns);
    }

    Destroy(gameObject);
  }
}
