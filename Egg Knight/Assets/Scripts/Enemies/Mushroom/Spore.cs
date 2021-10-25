using System.Collections;
using UnityEngine;

public class Spore : MonoBehaviour {
  private float _damage;

  private float _rotationSpeed;

  private Rigidbody2D _rb;
  private SpriteRenderer _sr;
  private float _initialAlpha;

  private void Awake() {
    _rotationSpeed = Random.Range(-0.1f, 0.1f);

    float randomScale = Random.Range(0.5f, 1.5f);
    transform.localScale = new Vector3(randomScale, randomScale, 1);

    _damage = 7.5f * randomScale;

    _rb = gameObject.GetComponent<Rigidbody2D>();
    _rb.velocity = new Vector2(Random.Range(-10f, 10f), Random.Range(-10f, 10f));

    _sr = gameObject.GetComponent<SpriteRenderer>();
    _initialAlpha = _sr.color.a;

    StartCoroutine(Despawn());
  }

  private void Update() {
    transform.Rotate(0, 0, _rotationSpeed);
  }

  private void OnTriggerEnter2D(Collider2D col) {
    if (col.CompareTag("Player")) {
      GameObject playerObject = col.gameObject;
      PlayerHealth playerHealth = playerObject?.GetComponent<PlayerHealth>();

      playerHealth?.Damage(_damage);
    }
  }

  private IEnumerator Despawn() {
    float r = _sr.color.r;
    float g = _sr.color.g;
    float b = _sr.color.b;

    float alpha = _sr.color.a;

    float despawnSpeed = Random.Range(0.01f, 0.05f);

    while (alpha > _initialAlpha/2) {
      alpha -= 0.005f;
      _sr.color = new Color(r, g, b, alpha);

      yield return new WaitForSeconds(despawnSpeed);
    }

    Destroy(gameObject);
  }
}
