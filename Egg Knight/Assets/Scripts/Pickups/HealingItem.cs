using UnityEngine;

public class HealingItem : MonoBehaviour {
  [SerializeField] private float _healAmount;

  private void OnTriggerEnter2D(Collider2D col) {
    if (col.CompareTag("Player")) {
      PlayerHealth playerHealth = col.gameObject.GetComponent<PlayerHealth>();

      playerHealth.Heal(_healAmount);

      Destroy(gameObject);
    }
  }
}
