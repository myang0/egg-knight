using UnityEngine;

public class BigPepper : BaseItem {
    [SerializeField] private float _speedBonus;
    [SerializeField] private float _attackSpeedMultiplier;

    protected override void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Player")) {
            GameObject pObject = col.gameObject;

            PlayerMovement pMovement = pObject?.GetComponent<PlayerMovement>();
            pMovement?.IncreaseMoveSpeed(_speedBonus);

            PlayerWeapons pWeapons = pObject?.GetComponent<PlayerWeapons>();
            pWeapons?.AddToSpeedMultiplier(_attackSpeedMultiplier);
            base.PickUp();
        }
    }
}