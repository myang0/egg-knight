using System;
using UnityEngine;

public class ItemDisplayEventArgs : EventArgs {
  public string name;
  public string description;
  public Sprite sprite;

  public ItemDisplayEventArgs(string name, string description, Sprite sprite) {
    this.name = name;
    this.description = description;
    this.sprite = sprite;
  }
}
