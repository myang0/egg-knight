using System;
using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour {
  [SerializeField] private GameObject _yolkPrefab;

  [SerializeField] private float _yolkCooldown = 2.0f;
  private bool _yolkOffCooldown = true;

  private PlayerInventory _inventory;

  [SerializeField] private GameObject _eggShardPrefab;
  private Coroutine _shardSpawn;
  private bool _shardsSpawning = false;

  private void Awake() {
    PlayerControls.OnRightClick += HandleSpaceBarPress;

    EggShards.OnEggShardsPickup += HandleEggShards;

    _inventory = gameObject.GetComponent<PlayerInventory>();
  }

  private void HandleSpaceBarPress(object sender, EventArgs e) {
    if (_yolkOffCooldown) {
      // Broadcast event that yolk has been shot

      StartCoroutine(ShootYolk());
    }
  }

  IEnumerator ShootYolk() {
    _yolkOffCooldown = false;

    Vector3 mousePosInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    Vector2 vectorToMouse = VectorHelper.GetVectorToPoint(transform.position, mousePosInWorld);

    float angleToMouse = Vector2.SignedAngle(Vector2.up, vectorToMouse);

    GameObject yolkObject = Instantiate(_yolkPrefab, transform.position, Quaternion.identity);
    YolkProjectile yolk = yolkObject.GetComponent<YolkProjectile>();
    yolk.SetDirection(vectorToMouse, angleToMouse);

    yield return new WaitForSeconds(_yolkCooldown);

    _yolkOffCooldown = true;
  }

  private void HandleEggShards(object sender, EventArgs e) {
    if (_shardsSpawning) {
      StopCoroutine(_shardSpawn);
    }

    _shardSpawn = StartCoroutine(SpawnShards(3 - _inventory.GetItemQuantity(Item.EggShards)));
  }

  private IEnumerator SpawnShards(float cooldown) {
    _shardsSpawning = true;

    while (true) {
      yield return new WaitForSeconds(cooldown);

      Instantiate(_eggShardPrefab, transform.position, Quaternion.identity);
    }
  }
}
