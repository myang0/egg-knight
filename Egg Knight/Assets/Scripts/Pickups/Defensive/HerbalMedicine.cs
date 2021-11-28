using UnityEngine;

public class HerbalMedicine : BaseItem {
  [SerializeField] private float _healMultiplier;

  protected override void OnTriggerEnter2D(Collider2D other) {
    if (other.CompareTag("Player")) {
      GameObject pObject = other.gameObject;

      PlayerHealth pHealth = pObject?.GetComponent<PlayerHealth>();
      pHealth?.ScaleHealMultiplier(_healMultiplier);

      base.PickUp();
    }
  }
}
