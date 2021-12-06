using Stage;
using System;
using System.Collections;
using UnityEngine;

public class EggnaBehaviour : EnemyBehaviour {
  [SerializeField] private float _meleeRange;
  [SerializeField] private float _longRange;

  private bool _isSpeaking = false;
  public bool IsSpeaking {
    get {
      return _isSpeaking;
    }
  }

  protected override void Awake() {
    EggnaHealth eggnaHealth = GetComponent<EggnaHealth>();
    eggnaHealth.OnEggnaStatusDamage += HandleStatusDamage;

    EnemyBehaviour enemyBehaviour = GetComponent<EnemyBehaviour>();
    enemyBehaviour.OnElectrocuted += HandleElectrocuted;

    LevelManager.OnEggnaHalfHealthDialogueStart += HandleDialogueStart;
    LevelManager.OnEggnaHalfHealthDialogueEnd += HandleDialogueEnd;

    Health = eggnaHealth;
    base.Awake();
  }

  private void HandleDialogueStart(object sender, EventArgs e) {
    _isSpeaking = true;
  }

  private void HandleDialogueEnd(object sender, EventArgs e) {
    _isSpeaking = false;
  }

  private void HandleElectrocuted(object sender, EventArgs e) {
    StartCoroutine(Electrocute());
  }

  protected override IEnumerator Electrocute() {
    yield break;
  }

  protected override IEnumerator AttackPlayer() {
    yield break;
  }

  public bool IsInMeleeRange() {
    return GetDistanceToPlayer() <= _meleeRange;
  }

  public bool IsInLongRange() {
    return GetDistanceToPlayer() > _longRange;
  }

  private void OnDestroy() {
    LevelManager.OnEggnaHalfHealthDialogueStart -= HandleDialogueStart;
    LevelManager.OnEggnaHalfHealthDialogueEnd -= HandleDialogueEnd;
  }
}
