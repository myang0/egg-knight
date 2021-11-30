using UnityEngine;

public class YolkShootPivot : MonoBehaviour {
  private Camera _mainCamera;

  private void Awake() {
    _mainCamera = Camera.main;
  }

  private void Update() {
    Vector3 mousePosInWorld = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
    Vector2 vectorToMouse = VectorHelper.GetVectorToPoint(transform.position, mousePosInWorld);

    float angle = Vector2.SignedAngle(Vector2.up, vectorToMouse);
    transform.eulerAngles = new Vector3(0, 0, angle);
  }
}
