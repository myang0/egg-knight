using UnityEngine;

public class DisplayWeapon : MonoBehaviour {
  private Camera _mainCamera;
  private SpriteRenderer _sr;

  private Transform _player;
  private PlayerInventory _inventory;

  private void Awake() {
    _mainCamera = Camera.main;
    _sr = gameObject.GetComponent<SpriteRenderer>();

    _player = GameObject.Find("Player").transform;
    _inventory = _player.GetComponent<PlayerInventory>();
  }

  private void FixedUpdate() {
    if (_inventory.HasItem(Item.QuailEgg)) transform.localScale = new Vector3(1.515152f, 1.515152f, 1.515152f);

    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    _sr.flipX = true;
    _sr.flipY = (mousePos.x > _player.position.x);
  }
}
