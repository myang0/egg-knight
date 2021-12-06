using System;
using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour {
  private PlayerInventory _inventory;

  [SerializeField] private GameObject _eggShardPrefab;
  private PlayerWeapons _playerWeapons;
  private Coroutine _shardSpawn;
  private bool _shardsSpawning = false;
  private float _eggShellCD;
  private const float _baseEggShellCD = 1f;

  private void Awake() {
    EggShards.OnEggShardsPickup += HandleEggShards;
    PlayerWeapons.OnDamageMultiplierChange += UpdateEggShellCD;
    _playerWeapons = gameObject.GetComponent<PlayerWeapons>();
    _inventory = gameObject.GetComponent<PlayerInventory>();
    _eggShellCD = _baseEggShellCD;
  }

  private void HandleEggShards(object sender, EventArgs e) {
    if (_shardsSpawning) {
      StopCoroutine(_shardSpawn);
    }
    _shardSpawn = StartCoroutine(SpawnShards());
  }

  private IEnumerator SpawnShards() {
    _shardsSpawning = true;

    while (true) {
      yield return new WaitForSeconds(_eggShellCD);

      Instantiate(_eggShardPrefab, transform.position, Quaternion.identity);
    }
  }

  private void UpdateEggShellCD(object sender, EventArgs e) {
    _eggShellCD = _baseEggShellCD / _playerWeapons.GetSpeedMultiplier();
  }

  private void OnDestroy() {
    EggShards.OnEggShardsPickup -= HandleEggShards;
    PlayerWeapons.OnDamageMultiplierChange -= UpdateEggShellCD;
  }
}
