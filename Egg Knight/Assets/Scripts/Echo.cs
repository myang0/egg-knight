using UnityEngine;

public class Echo : MonoBehaviour {
  public void OnEchoEnd() {
    Destroy(gameObject);
  }  
}
