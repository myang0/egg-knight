using System.Collections;
using UnityEngine;

public class GunSprite : MonoBehaviour {
  protected Animator _anim;
  protected SpriteRenderer _sr;

  [SerializeField] private Transform _enemyTransform;
  private Transform _playerTransform;

  protected virtual void Awake() {
    _anim = GetComponent<Animator>();
    _sr = GetComponent<SpriteRenderer>();

    _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
  }

  private void Update() {
    _sr.flipX = (_playerTransform.position.x < _enemyTransform.position.x);
  }

  protected IEnumerator FadeIn() {
    _sr.enabled = true;

    float r = _sr.color.r;
    float g = _sr.color.g;
    float b = _sr.color.b;
    float a = _sr.color.a;

    while (a < 1) {
      a += 0.05f;
      _sr.color = new Color(r, g, b, a);

      yield return new WaitForSeconds(0.025f);
    }
  }

  protected IEnumerator FadeOut() {
    float r = _sr.color.r;
    float g = _sr.color.g;
    float b = _sr.color.b;
    float a = _sr.color.a;

    while (a > 0) {
      a -= 0.05f;
      _sr.color = new Color(r, g, b, a);

      yield return new WaitForSeconds(0.025f);
    }

    _sr.enabled = false;
  }
}
