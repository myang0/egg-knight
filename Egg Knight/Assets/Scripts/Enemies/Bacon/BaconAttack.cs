using UnityEngine;

public class BaconAttack : MonoBehaviour {
  [SerializeField] private GameObject _greaseOrbPrefab;

  public void SpawnOrb() {
    if (_greaseOrbPrefab != null) {
      Instantiate(_greaseOrbPrefab, transform.position, Quaternion.identity);
    }
  }
}
