using UnityEngine;

public class Crosshair : MonoBehaviour {
  private Animator _anim;

  private void Awake() {
    _anim = GetComponent<Animator>();
  }

  public void FadeOut() {
    _anim.Play("CrosshairFadeOut");
  }

  public void OnAnimationEnd() {
    Destroy(gameObject);
  }  
}
