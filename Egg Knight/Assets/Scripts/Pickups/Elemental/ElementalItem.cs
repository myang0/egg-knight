using System;
using UnityEngine;

public class ElementalItem : BaseItem
{
  [SerializeField] private StatusCondition element;

  public static event EventHandler<ElementalItemEventArgs> OnElementalItemPickup;
  
  protected override void OnTriggerEnter2D(Collider2D col) {
    if (col.CompareTag("Player")) {
      OnElementalItemPickup?.Invoke(this, new ElementalItemEventArgs(element));

      PickUp();
    }
  }
}
