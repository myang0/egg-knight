using System.Collections;
using UnityEngine;
using Cinemachine;

public class VirtualCamera : MonoBehaviour {
  [SerializeField] private CinemachineVirtualCamera _virtualCamera;

  private static VirtualCamera _instance;
  public static VirtualCamera Instance {
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

  public void Shake(float intensity, float time) {
    StartCoroutine(ShakeTime(intensity, time));
  }

  private IEnumerator ShakeTime(float intensity, float time) {
    CinemachineBasicMultiChannelPerlin perlin = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

    perlin.m_AmplitudeGain = intensity;

    yield return new WaitForSeconds(time);

    perlin.m_AmplitudeGain = 0;
  }
}
