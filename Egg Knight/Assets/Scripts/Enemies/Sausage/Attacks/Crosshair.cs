using System;
using UnityEngine;

public class Crosshair : MonoBehaviour {
  private Animator _anim;

  private void Awake() {
    _anim = GetComponent<Animator>();

    SausageHealth.OnSausageDeath += HandleSausageDeath;
  }

  private void HandleSausageDeath(object sender, EventArgs e) {
    FadeOut();
  }

  public void FadeOut() {
    _anim.Play("CrosshairFadeOut");
  }

  public void OnAnimationEnd() {
    Destroy(gameObject);
  }

  private void OnDestroy() {
    SausageHealth.OnSausageDeath -= HandleSausageDeath;
  }
}
