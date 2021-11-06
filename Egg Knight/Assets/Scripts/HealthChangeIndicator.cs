using UnityEngine;

public class HealthChangeIndicator : MonoBehaviour {
  private Rigidbody2D _rb;
  private TextMesh _textmesh;

  [SerializeField] private int _minFontSize;
  [SerializeField] private int _maxFontSize;
  
  private void Awake() {
    Vector2 forceVector = Quaternion.Euler(0, 0, Random.Range(-45, 45)) * Vector2.up;

    _rb = gameObject.GetComponent<Rigidbody2D>();
    _textmesh = gameObject.GetComponent<TextMesh>();

    _rb.AddForce(forceVector * Random.Range(5f, 15f), ForceMode2D.Impulse);
    
    var position = transform.position;
    transform.position = new Vector3(position.x, position.y, ZcoordinateConsts.OverCharacter);
  }

  public void Initialize(float value, Color color) {
    _textmesh.text = $"{(int)Mathf.Round(value)}";

    float clampedValue = Mathf.Clamp(value, 0f, 100f);
    int fontSize = (int)Mathf.Round(((clampedValue / 100f) * (_maxFontSize - _minFontSize)) + _minFontSize);

    _textmesh.fontSize = fontSize;
    
    _textmesh.color = color;
  }

  public void OnAnimationEnd() {
    Destroy(gameObject);
  }
}
