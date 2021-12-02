using UnityEngine;

public class SoundPlayer : MonoBehaviour {
  [SerializeField] private GameObject _singleTimeSound;

  public void PlayClipWithScaledVolume(AudioClip clip) {}

  public void PlayClip(AudioClip clip, float volumeScaling = 1.0f) {
    if (_singleTimeSound != null) {
      SingleTimeSound sound = Instantiate(_singleTimeSound, transform.position, Quaternion.identity)
        .GetComponent<SingleTimeSound>();

      sound.ScaleVolume(volumeScaling);
      sound.LoadClipAndPlay(clip);
    }
  }
}
