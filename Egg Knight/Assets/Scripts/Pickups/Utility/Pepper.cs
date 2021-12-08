using UnityEngine;

public class Pepper : BaseItem {
  [SerializeField] private float _speedBonus;

  protected override void OnTriggerEnter2D(Collider2D col) {
    if (col.CompareTag("Player")) {
      GameObject pObject = col.gameObject;

      PlayerMovement pMovement = pObject?.GetComponent<PlayerMovement>();
      pMovement?.IncreaseMoveSpeed(_speedBonus);

      base.PickUp();
    }
  }
}
