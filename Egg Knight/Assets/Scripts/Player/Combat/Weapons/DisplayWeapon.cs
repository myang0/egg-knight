using System;
using UnityEngine;

public class DisplayWeapon : MonoBehaviour {
  private Camera _mainCamera;
  private SpriteRenderer _sr;
  private bool _enableFlip;
  private Transform _player;
  private PlayerInventory _inventory;

  private void Awake() {
    _mainCamera = Camera.main;
    _sr = gameObject.GetComponent<SpriteRenderer>();
    _enableFlip = true;
    _player = GameObject.Find("Player").transform;
    _inventory = _player.GetComponent<PlayerInventory>();
    PlayerHealth.OnGameOver += DisableFlip;
    PlayerHealth.OnReviveGameOver += DisableFlip;
    PlayerDeath.OnReviveSequenceDone += EnableFlip;
  }

  private void FixedUpdate() {
    if (_inventory.HasItem(Item.QuailEgg)) transform.localScale = new Vector3(1.515152f, 1.515152f, 1.515152f);

    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    _sr.flipX = true;
    if (!_enableFlip) return;
    _sr.flipY = mousePos.x > _player.position.x;
  }

  private void EnableFlip(object sender, EventArgs e) {
    _enableFlip = true;
  }
  
  private void DisableFlip(object sender, EventArgs e) {
    _enableFlip = false;
  }

  private void OnDestroy() {
    PlayerHealth.OnGameOver -= DisableFlip;
    PlayerHealth.OnReviveGameOver -= DisableFlip;
    PlayerDeath.OnReviveSequenceDone -= EnableFlip;
  }
}
