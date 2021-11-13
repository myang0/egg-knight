using System.Collections;
using UnityEngine;

public class SausageBombAttack : MonoBehaviour {
  [SerializeField] private int _totalThrows;
  [SerializeField] private float _timeBetweenThrows;
  [SerializeField] private int _bombsPerThrow;

  [SerializeField] private GameObject _bombObject;

  private Transform _playerTransform;

  public void StartAttack() {
    StartCoroutine(Bomb());
  }

  private IEnumerator Bomb() {
    for (int i = 0; i < _totalThrows; i++) {
      for (int j = 0; j < _bombsPerThrow; j++) {
        Instantiate(_bombObject, transform.position, Quaternion.identity);
      }

      yield return new WaitForSeconds(_timeBetweenThrows);
    }
  }
}
