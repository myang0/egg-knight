using System;
using System.Collections;
using UnityEngine;

public class SheriffRifleSprite : GunSprite {
  protected override void Awake() {
    SausageSnipeAttack.OnAttackStart += HandleAttackStart;
    SausageSnipeAttack.OnRifleShot += HandleShot;
    SausageSnipeAttack.OnAttackEnd += HandleAttackEnd;

    base.Awake();
  }

  private void HandleAttackStart(object sender, EventArgs e) {
    StartCoroutine(FadeIn(0.01f));
  }

  private void HandleShot(object sender, EventArgs e) {
    _anim.Play("RifleShoot");
  }

  private void HandleAttackEnd(object sender, EventArgs e) {
    StartCoroutine(FadeOut(0.05f));
  }

  private void OnDestroy() {
    SausageSnipeAttack.OnAttackStart -= HandleAttackStart;
    SausageSnipeAttack.OnRifleShot -= HandleShot;
    SausageSnipeAttack.OnAttackEnd -= HandleAttackEnd;
  }
}
