using UnityEngine;

public class DisplayWeapon : MonoBehaviour {
  private Camera _mainCamera;
  private SpriteRenderer _sr;

  private Transform _player;

  private void Awake() {
    _mainCamera = Camera.main;
    _sr = gameObject.GetComponent<SpriteRenderer>();

    _player = GameObject.Find("Player").transform;
  }

  private void FixedUpdate() {
    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    _sr.flipX = true;
    _sr.flipY = (mousePos.x > _player.position.x);
  }
}
