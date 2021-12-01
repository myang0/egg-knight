using UnityEngine;

public class BrandNewHelmet : BaseItem {
  [SerializeField] private float _armourModifier;

  protected override void OnTriggerEnter2D(Collider2D other) {
    if (other.CompareTag("Player")) {
      GameObject pObject = other.gameObject;

      PlayerHealth pHealth = pObject?.GetComponent<PlayerHealth>();
      pHealth?.ScaleArmour(_armourModifier);

      base.PickUp();
    }
  }
}
