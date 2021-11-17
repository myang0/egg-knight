using UnityEngine;

public class RoyalEggSlash : MonoBehaviour {
  [SerializeField] private float _damage;

  private SpriteRenderer _sr;

  private void Awake() {
    _sr = GetComponent<SpriteRenderer>();
  }

  public void SetRotation(float angle) {
    transform.eulerAngles = new Vector3(0, 0, angle);
  }
}
