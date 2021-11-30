using System;
using UnityEngine;

public class Crosshair : MonoBehaviour {
  private Animator _anim;

  private void Awake() {
    _anim = GetComponent<Animator>();

    SausageHealth.OnSausageDeath += HandleBossDeath;
  }

  private void HandleBossDeath(object sender, EventArgs e) {
    FadeOut();
  }

  public void FadeOut() {
    _anim.Play("CrosshairFadeOut");
  }

  public void OnAnimationEnd() {
    Destroy(gameObject);
  }  
}
