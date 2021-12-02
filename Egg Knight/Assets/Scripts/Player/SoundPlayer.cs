using UnityEngine;

public class SoundPlayer : MonoBehaviour {
  [SerializeField] private GameObject _singleTimeSound;

  public void PlayClipWithScaledVolume(AudioClip clip) {}

  public void PlayClip(AudioClip clip, float volumeScaling = 1.0f, float pitchScaling = 1.0f, float minPitch = 0.75f, float maxPitch = 1.25f) {
    if (_singleTimeSound != null) {
      SingleTimeSound sound = Instantiate(_singleTimeSound, transform.position, Quaternion.identity)
        .GetComponent<SingleTimeSound>();

      sound.ScaleVolume(volumeScaling);
      sound.ScalePitch(pitchScaling);
      sound.RandomizePitch(minPitch, maxPitch);
      sound.LoadClipAndPlay(clip);
    }
  }
}
