using System;
using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour {
  private PlayerInventory _inventory;

  [SerializeField] private GameObject _eggShardPrefab;
  private Coroutine _shardSpawn;
  private bool _shardsSpawning = false;

  private void Awake() {
    EggShards.OnEggShardsPickup += HandleEggShards;

    _inventory = gameObject.GetComponent<PlayerInventory>();
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

  private void OnDestroy() {
    EggShards.OnEggShardsPickup -= HandleEggShards;
  }
}
