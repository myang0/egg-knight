using UnityEngine;

public class Echo : MonoBehaviour {
  private void Awake() {
    Transform _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    GetComponent<SpriteRenderer>().flipX = transform.position.x > _playerTransform.position.x;
  }

  public void OnEchoEnd() {
    Destroy(gameObject);
  }  
}
