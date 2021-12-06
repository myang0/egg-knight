using UnityEngine;

public class Milk : BaseItem {
  [SerializeField] private float _armourModifier;
  [SerializeField] private float _speedMultiplier;

  protected override void OnTriggerEnter2D(Collider2D other) {
    if (other.CompareTag("Player")) {
      GameObject pObject = other.gameObject;

      PlayerHealth pHealth = pObject?.GetComponent<PlayerHealth>();
      pHealth?.ScaleArmour(_armourModifier);
      
      PlayerMovement pMovement = pObject?.GetComponent<PlayerMovement>();
      pMovement?.MultiplyMoveSpeed(_speedMultiplier);

      base.PickUp();
    }
  }
}
