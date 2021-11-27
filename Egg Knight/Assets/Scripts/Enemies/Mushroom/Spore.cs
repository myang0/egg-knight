using System.Collections;
using UnityEngine;

public class Spore : MonoBehaviour {
  private float _damage;

  private float _rotationSpeed;

  private Rigidbody2D _rb;
  private SpriteRenderer _sr;
  private Collider2D _collider;

  private float _initialAlpha;

  private void Awake() {
    _rotationSpeed = Random.Range(-0.1f, 0.1f);

    float randomScale = Random.Range(0.5f, 2f);
    transform.localScale = new Vector3(randomScale, randomScale, 1);

    _damage = 4f * randomScale;

    _rb = gameObject.GetComponent<Rigidbody2D>();
    _rb.velocity = new Vector2(Random.Range(-10f, 10f), Random.Range(-10f, 10f));

    _sr = gameObject.GetComponent<SpriteRenderer>();
    _initialAlpha = _sr.color.a;

    _collider = GetComponent<Collider2D>();

    StartCoroutine(Fade());
  }

  private void Update() {
    transform.Rotate(0, 0, _rotationSpeed);
  }

  private void OnTriggerEnter2D(Collider2D col) {
    if (col.CompareTag("Player")) {
      GameObject playerObject = col.gameObject;
      PlayerHealth playerHealth = playerObject?.GetComponent<PlayerHealth>();

      playerHealth?.Damage(Mathf.Round(_damage));
    }
  }

  private IEnumerator Fade() {
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

    StartCoroutine(Despawn());
  }

  private IEnumerator Despawn() {
    _collider.enabled = false;

    float r = _sr.color.r;
    float g = _sr.color.g;
    float b = _sr.color.b;

    float alpha = _sr.color.a;

    while (alpha > 0) {
      alpha -= 0.02f;
      _sr.color = new Color(r, g, b, alpha);

      float currentScale = transform.localScale.x;
      if (currentScale > 0) {
        transform.localScale = (currentScale - 0.02f >= 0) ? new Vector3(currentScale - 0.02f, currentScale - 0.02f, 1) : Vector3.forward;
      }

      yield return new WaitForSeconds(0.01f);
    }

    Destroy(gameObject);
  }
}
