using System;
using UnityEngine;

public class MovementVectorEventArgs : EventArgs {
  public Vector2 vector;

  public MovementVectorEventArgs(Vector2 vector) {
    this.vector = vector;
  }
}
