using UnityEngine;

public class NPC : MonoBehaviour {
  private SpriteRenderer _sr;
  private Transform _playerTransform;

  private void Awake() {
    _sr = GetComponent<SpriteRenderer>();

    _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
  }

  private void Update() {
    _sr.flipX = transform.position.x - _playerTransform.position.x > 0;
  }
}
