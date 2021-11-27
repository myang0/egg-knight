using UnityEngine;

public static class ProjectileHelper {
  public static void Refrigerate(PlayerInventory inventory, Projectile p) {
    if (inventory == null || p == null) {
      return;
    }

    p.MultiplySpeed((inventory.HasItem(Item.PortableRefrigerator) ? 0.75f : 1.0f));
  }
}
