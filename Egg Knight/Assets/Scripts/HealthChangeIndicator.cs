using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class HealthChangeIndicator : MonoBehaviour {
  private Rigidbody2D _rb;
  private TextMesh _textmesh;

  [SerializeField] private int _minFontSize;
  [SerializeField] private int _maxFontSize;

  private bool _isCritical;
  
  private void Awake() {
    Vector2 forceVector = Quaternion.Euler(0, 0, Random.Range(-45, 45)) * Vector2.up;

    _rb = gameObject.GetComponent<Rigidbody2D>();
    _textmesh = gameObject.GetComponent<TextMesh>();

    _rb.AddForce(forceVector * Random.Range(5f, 15f), ForceMode2D.Impulse);
    
    var position = transform.position;
    transform.position = new Vector3(position.x, position.y, ZcoordinateConsts.UIlike);
  }

  private void FixedUpdate() {
    if (_isCritical) {
      _textmesh.color = (_textmesh.color == Color.red) ? Color.yellow : Color.red;
    }
  }

  public void InitializeCritical(float value) {
    string valueText = (value < 1.0f) ? value.ToString("0.##!") : value.ToString("#.##!");
    _textmesh.text = valueText;

    _textmesh.fontSize = (int)(CalculateFontSize(value) * 1.25f);
    _textmesh.color = Color.red;

    _isCritical = true;
  }

  public void Initialize(float value, Color color) {
    string valueText = (value < 1.0f) ? value.ToString("0.##") : value.ToString("#.##");
    _textmesh.text = valueText;

    _textmesh.fontSize = CalculateFontSize(value);
    _textmesh.color = color;

    if (value == 0) _textmesh.color = Color.gray;
  }

  private int CalculateFontSize(float value) {
    float clampedValue = Mathf.Clamp(value, 0f, 100f);
    return (int)Mathf.Round(((clampedValue / 100f) * (_maxFontSize - _minFontSize)) + _minFontSize);
  }

  public void OnAnimationEnd() {
    Destroy(gameObject);
  }
}
