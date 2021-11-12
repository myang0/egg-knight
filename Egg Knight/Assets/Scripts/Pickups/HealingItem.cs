using UnityEngine;

public class HealingItem : PickupBase {
  [SerializeField] protected float healAmount;

  // private void OnTriggerEnter2D(Collider2D col) {
  //   if (col.CompareTag("Player")) {
  //     PickUp(col.gameObject);
  //   }
  // }

  public override void PickUp(GameObject playerObject) {
    playerObject.GetComponent<PlayerHealth>().Heal(healAmount);
    Destroy(gameObject);
  }
}
