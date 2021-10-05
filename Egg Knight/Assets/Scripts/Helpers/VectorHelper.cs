using UnityEngine;

public static class VectorHelper {
  public static Vector2 GetVectorToPoint(Vector3 source, Vector3 destination) {
    Vector2 source2D = new Vector2(source.x, source.y);
    Vector2 destination2D = new Vector2(destination.x, destination.y);

    return (destination2D - source2D).normalized;
  }
}