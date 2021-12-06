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
  private int _maxTeleportTries = 30;

  private Animator _anim;
  private SpriteRenderer _sr;
  private Collider2D _collider;

  [SerializeField] private GameObject _singleTimeSound;
  [SerializeField] private AudioClip _clip;

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

    SingleTimeSound sound = Instantiate(_singleTimeSound, transform.position, Quaternion.identity)
      .GetComponent<SingleTimeSound>();

    sound.ScaleVolume(0.5f);
    sound.LoadClipAndPlay(_clip);

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

    SingleTimeSound sound = Instantiate(_singleTimeSound, transform.position, Quaternion.identity)
      .GetComponent<SingleTimeSound>();

    sound.ScaleVolume(0.5f);
    sound.LoadClipAndPlay(_clip);

    Instantiate(_smokeParticles, transform.position, Quaternion.identity);
    
    _sr.enabled = true;
    _collider.enabled = true;
  }

  private Vector3 FindNewPosition() {
    if (_teleportPoints.Any()) {
      int randomIndex = Random.Range(0, _teleportPoints.Count);
      float distance = Vector3.Distance(transform.position, _teleportPoints[randomIndex].GetPosition());

      int tries = 0;

      while (distance >= _maxTeleportDistance && distance <= _minTeleportDistance && tries < _maxTeleportTries) {
        randomIndex = Random.Range(0, _teleportPoints.Count);
        distance = Vector3.Distance(transform.position, _teleportPoints[randomIndex].GetPosition());

        tries++;
      }

      if (tries >= _maxTeleportTries) {
        return transform.position;
      } else {
        return _teleportPoints[randomIndex].GetPosition();
      }
    } else {
      return new Vector3(
        transform.position.x + Random.Range(-5f, 5f),
        transform.position.y + Random.Range(-5f, 5f),
        transform.position.z
      );
    }
  }
}
