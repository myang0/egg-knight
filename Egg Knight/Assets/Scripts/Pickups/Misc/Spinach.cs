using UnityEngine;

public class Spinach : BaseItem {
  [SerializeField] private float _bonusHealth;
  [SerializeField] private float _bonusDamage;
  [SerializeField] private float _attackSpeedMultiplier;
  [SerializeField] private float _speedMultiplier;

  protected override void OnTriggerEnter2D(Collider2D col) {
    if (col.CompareTag("Player")) {
      GameObject pObject = col.gameObject;

      PlayerMovement pMovement = pObject?.GetComponent<PlayerMovement>();
      PlayerHealth pHealth = pObject?.GetComponent<PlayerHealth>();
      PlayerWeapons pWeapons = pObject?.GetComponent<PlayerWeapons>();

      pMovement?.MultiplyMoveSpeed(_speedMultiplier);
      pHealth?.AddToMaxHealth(_bonusHealth);
      pWeapons?.AddToSpeedMultiplier(_attackSpeedMultiplier);
      pWeapons?.AddToDamageMultiplier(_bonusDamage);

      base.PickUp();
    }
  }
}
