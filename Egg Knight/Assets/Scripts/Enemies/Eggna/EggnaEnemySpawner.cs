using Stage;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EggnaEnemySpawner : MonoBehaviour {
  [SerializeField] private List<GameObject> _enemies;

  private List<EnemySpawnpoint> _spawnPoints = new List<EnemySpawnpoint>();
  private bool _isSpawning = false;

  [SerializeField] private float _minDistanceToPlayer = 10f;

  [SerializeField] private GameObject _smokeParticles;

  private Transform _playerTransform;

  [SerializeField] private int _maxEnemiesSpawned = 3;
  private int _numEnemiesSpawned = 0;

  private void Awake() {
    _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

    EggnaHealth.OnEggnaBelowHalfHealth += HandleEggnaBelowHalfHealth;
    EggnaHealth.OnEggnaDeath += HandleEggnaDeath;
  }

  private void HandleEggnaBelowHalfHealth(object sender, EventArgs e) {
    _isSpawning = true;

    GameObject levelManager = GameObject.FindGameObjectWithTag("LevelManager");
    if (levelManager != null) {
      _spawnPoints = levelManager.GetComponent<LevelManager>()
        .GetCurrentStage()
        .GetEnemySpawnPoints();
    }

    StartCoroutine(EnemySpawnCycle());
  }

  private void HandleEggnaDeath(object sender, EventArgs e) {
    StopCoroutine(EnemySpawnCycle());

    _isSpawning = false;
  }

  private IEnumerator EnemySpawnCycle() {
    while (_isSpawning) {
      if (_numEnemiesSpawned < _maxEnemiesSpawned) {
        SpawnEnemy();
      }

      yield return new WaitForSeconds(10);
    }
  }

  private void SpawnEnemy() {
    Vector3 spawnPos = GetSpawnPosition();

    int randomIndex = Random.Range(0, _enemies.Count);

    Instantiate(_smokeParticles, spawnPos, Quaternion.identity);
    EnemyBehaviour eBehaviour = Instantiate(_enemies[randomIndex], spawnPos, Quaternion.identity).GetComponent<EnemyBehaviour>();
    eBehaviour.decrementEnemyCountOnDeath = false;
    eBehaviour.spawnedByEggna = true;

    _numEnemiesSpawned++;
  }

  private Vector3 GetSpawnPosition() {
    int randomIndex = Random.Range(0, _spawnPoints.Count);

    float spawnPointDistanceToPlayer = Vector3.Distance(transform.position, _spawnPoints[randomIndex].GetPosition());

    while (spawnPointDistanceToPlayer < _minDistanceToPlayer) {
      randomIndex = Random.Range(0, _spawnPoints.Count);
      spawnPointDistanceToPlayer = Vector3.Distance(transform.position, _spawnPoints[randomIndex].GetPosition());
    }

    return _spawnPoints[randomIndex].GetPosition();
  }

  private void OnDestroy() {
    EggnaHealth.OnEggnaBelowHalfHealth -= HandleEggnaBelowHalfHealth;
    EggnaHealth.OnEggnaDeath -= HandleEggnaDeath;
  }

  public void DecrementEnemies() {
    _numEnemiesSpawned--;
  }
}
