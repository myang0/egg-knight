using UnityEngine;

public class DuckEgg : BaseItem {
  [SerializeField] private float _healthBonus;

  protected override void OnTriggerEnter2D(Collider2D other) {
    if (other.CompareTag("Player")) {
      GameObject pObject = other.gameObject;

      PlayerHealth pHealth = pObject?.GetComponent<PlayerHealth>();
      pHealth?.AddToMaxHealth(_healthBonus);
      pHealth?.Heal(_healthBonus);

      base.PickUp();
    }
  }
}
