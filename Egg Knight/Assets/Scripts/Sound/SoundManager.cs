using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
  [SerializeField] private GameObject _singleTimeSound;
  [SerializeField] private List<SoundAudioClip> _soundAudioClips;

  private static SoundManager _instance;
  public static SoundManager Instance {
    get {
      return _instance;
    }
  }

  private void Awake() {
    if (_instance != null && _instance != this) {
      Destroy(this.gameObject);
    } else {
      _instance = this;
    }
  }

  public void PlaySound(Sound sound) {
    AudioClip clip = GetClip(sound);

    if (clip != null) {
      SingleTimeSound stSound = Instantiate(_singleTimeSound, transform.position, Quaternion.identity).GetComponent<SingleTimeSound>();

      if (sound == Sound.Pierce) {
        stSound.RandomizePitch(0.9f, 1.1f);
      }

      stSound.LoadClipAndPlay(clip);
    }
  }

  private AudioClip GetClip(Sound sound) {
    foreach (SoundAudioClip s in _soundAudioClips) {
      if (sound == s.sound) {
        return s.clip;
      }
    }
    
    return null;
  }  
}
