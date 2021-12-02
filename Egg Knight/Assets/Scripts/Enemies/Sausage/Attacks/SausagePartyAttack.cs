using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class SausagePartyAttack : MonoBehaviour {
  [SerializeField] private GameObject _minionObject;

  private SoundPlayer _soundPlayer;
  [SerializeField] private AudioClip _partyClip;

  [SerializeField] private List<Transform> _spawnPoints;

  public static int Partygoers = 0;

  private void Awake() {
    _soundPlayer = GetComponent<SoundPlayer>();

    Partygoers = 0;
  }

  public void StartParty() {
    _soundPlayer.PlayClip(_partyClip);

    int minionsToSpawn = Random.Range(1, 4);

    for (int i = 0; i < minionsToSpawn; i++) {
      Instantiate(_minionObject, _spawnPoints[i].position, Quaternion.identity);
      Partygoers++;
    }
  }
}
