using System;
using UnityEngine;

public class EnemyStatusParticles : MonoBehaviour {
  [SerializeField] private GameObject _yolkParticles;
  [SerializeField] private GameObject _igniteParticles;
  [SerializeField] private GameObject _frostParticles;
  [SerializeField] private GameObject _electrocuteParticles;
  [SerializeField] private GameObject _bleedParticles;

  private void Awake() {
    EnemyBehaviour enemyBehaviour = gameObject.GetComponent<EnemyBehaviour>();
    enemyBehaviour.OnYolked += HandleYolked;
    enemyBehaviour.OnIgnited += HandleIgnited;
    enemyBehaviour.OnFrosted += HandleFrosted;
    enemyBehaviour.OnElectrocuted += HandleElectrocuted;
    enemyBehaviour.OnBleed += HandleBleed;
  }

  private void HandleYolked(object sender, EventArgs e) {
    if (_yolkParticles != null) {
      Instantiate(_yolkParticles, transform.position, Quaternion.identity);
    }
  }

  private void HandleIgnited(object sender, EventArgs e) {
    if (_igniteParticles != null) {
      Instantiate(_igniteParticles, transform.position, Quaternion.identity);
    }
  }

  private void HandleFrosted(object sender, EventArgs e) {
    if (_frostParticles != null) {
      Instantiate(_frostParticles, transform.position, Quaternion.identity);
    }
  } 

  private void HandleElectrocuted(object sender, EventArgs e) {
    if (_electrocuteParticles != null) {
      Instantiate(_electrocuteParticles, transform.position, Quaternion.identity);
    }
  }

  private void HandleBleed(object sender, EventArgs e) {
    if (_bleedParticles != null) {
      Instantiate(_bleedParticles, transform.position, Quaternion.identity);
    }
  }
}
