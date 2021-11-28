using System.Collections.Generic;
using UnityEngine;

public class YolkExplosion : Explosion {
  private YolkUpgradeManager _upgrades;

  public void SetUpgrades(YolkUpgradeManager upgrades) {
    _upgrades = upgrades;
  }

  public void MultiplyDamage(float damageMultiplier) {
    _explosionDamage *= damageMultiplier;
    _explosionRange *= damageMultiplier;

    transform.localScale = new Vector3(damageMultiplier, damageMultiplier, damageMultiplier);
  }

  public override void OnExplode() {
    Collider2D[] entitiesInRange = Physics2D.OverlapCircleAll(transform.position, _explosionRange, _hitLayer);

    List<StatusCondition> statusList;
    if (_upgrades.HasUpgrade(YolkUpgradeType.CorrosiveCore)) {
      statusList = new List<StatusCondition>() { StatusCondition.Yolked, StatusCondition.Weakened };
    } else {
      statusList = new List<StatusCondition>() { StatusCondition.Yolked };
    }

    foreach (Collider2D entity in entitiesInRange) {
      EnemyHealth eHealth = entity.GetComponent<EnemyHealth>();

      eHealth?.DamageWithStatuses(_explosionDamage, statusList);
    }
  }
}
