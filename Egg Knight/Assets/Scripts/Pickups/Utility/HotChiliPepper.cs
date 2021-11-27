using UnityEngine;

public class HotChiliPepper : BaseItem {
  [SerializeField] private float _speedMultiplier;

  protected override void OnTriggerEnter2D(Collider2D col) {
    if (col.CompareTag("Player")) {
      GameObject pObject = col.gameObject;

      PlayerMovement pMovement = pObject?.GetComponent<PlayerMovement>();
      pMovement?.MultiplyMoveSpeed(_speedMultiplier);

      base.PickUp();
    }
  }
}
