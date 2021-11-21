using System;
using UnityEngine;

public class MinionRevolverSprite : GunSprite {
  [SerializeField] private SausageMinionHealth _sausageMinionHealth;

  protected override void Awake() {
    if (_sausageMinionHealth != null) {
      _sausageMinionHealth.OnSausageMinionDeath += HandleDeath;
    }

    base.Awake();
  }

  private void HandleDeath(object sender, EventArgs e) {
    _sr.enabled = false;
  }

  public void PlayAnimation() {
    _anim.Play("RevolverShoot");
  }  
}
