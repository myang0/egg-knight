using UnityEngine;

public class BroccoliActivator : MonoBehaviour {
  [SerializeField] private Collider2D _activationCollider;

  private bool _hasDialoguePlayed = false;

  private void OnTriggerEnter2D(Collider2D other) {
    if (other.CompareTag("Player")) {
      if (_hasDialoguePlayed == false) {
        if (GameObject.Find("Flowchart")) {
          Fungus.Flowchart.BroadcastFungusMessage("BrigandBroccoliStart");
        } else {
          GetComponent<Animator>().SetBool("IsActive", true);
        }
        
        _hasDialoguePlayed = true;
        _activationCollider.enabled = false;
      }
    }
  }  
}
