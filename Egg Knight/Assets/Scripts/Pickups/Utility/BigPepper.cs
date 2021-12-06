using UnityEngine;

public class BigPepper : BaseItem {
    [SerializeField] private float _speedMultiplier;
    [SerializeField] private float _attackSpeedMultiplier;

    protected override void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Player")) {
            GameObject pObject = col.gameObject;

            PlayerMovement pMovement = pObject?.GetComponent<PlayerMovement>();
            pMovement?.MultiplyMoveSpeed(_speedMultiplier);

            PlayerWeapons pWeapons = pObject?.GetComponent<PlayerWeapons>();
            pWeapons?.MultiplySpeed(_attackSpeedMultiplier);
            base.PickUp();
        }
    }
}