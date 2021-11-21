using System;
using System.Collections;
using UnityEngine;

public class SheriffRevolverSprite : GunSprite {
  protected override void Awake() {
    SausageWalkAttack.OnAttackStart += HandleAttackStart;
    SausageWalkAttack.OnRevolverShot += HandleShot;
    SausageWalkAttack.OnAttackEnd += HandleAttackEnd;

    base.Awake();
  }

  private void HandleAttackStart(object sender, EventArgs e) {
    StartCoroutine(FadeIn());
  }

  private void HandleShot(object sender, EventArgs e) {
    _anim.Play("RevolverShoot");
  }

  private void HandleAttackEnd(object sender, EventArgs e) {
    StartCoroutine(FadeOut());
  }

  private void OnDestroy() {
    SausageWalkAttack.OnAttackStart -= HandleAttackStart;
    SausageWalkAttack.OnRevolverShot -= HandleShot;
    SausageWalkAttack.OnAttackEnd -= HandleAttackEnd;
  }
}
