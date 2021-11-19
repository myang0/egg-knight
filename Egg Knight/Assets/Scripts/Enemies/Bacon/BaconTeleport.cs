using Stage;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaconTeleport : MonoBehaviour {
  [SerializeField] private float _minTeleportDistance;
  [SerializeField] private float _maxTeleportDistance;

  [SerializeField] private GameObject _smokeParticles;
  [SerializeField] private float _invisibleTime;

  private List<EnemySpawnpoint> _teleportPoints = new List<EnemySpawnpoint>();

  private Animator _anim;
  private SpriteRenderer _sr;
  private Collider2D _collider;

  private void Awake() {
    _anim = GetComponent<Animator>();
    _sr = GetComponent<SpriteRenderer>();
    _collider = GetComponent<Collider2D>();

    GameObject levelManager = GameObject.FindGameObjectWithTag("LevelManager");
    if (levelManager != null) {
      _teleportPoints = levelManager.GetComponent<LevelManager>()
        .GetCurrentStage()
        .GetEnemySpawnPoints();
    }
  }

  public void Disappear() {
    Instantiate(_smokeParticles, transform.position, Quaternion.identity);

    _sr.enabled = false;
    _collider.enabled = false;

    StartCoroutine(Invisibility());
  }

  private IEnumerator Invisibility() {
    yield return new WaitForSeconds(_invisibleTime);

    _anim.SetBool("IsTeleportingIn", true);
  }

  public void Reappear() {
    transform.position = FindNewPosition();

    Instantiate(_smokeParticles, transform.position, Quaternion.identity);
    
    _sr.enabled = true;
    _collider.enabled = true;
  }

  private Vector3 FindNewPosition() {
    if (_teleportPoints.Any()) {
      int randomIndex = Random.Range(0, _teleportPoints.Count);
      return _teleportPoints[randomIndex].GetPosition();
    } else {
      return transform.position;
    }
  }
}
