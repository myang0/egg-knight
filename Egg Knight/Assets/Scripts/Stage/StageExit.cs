using System;
using UnityEngine;

namespace Stage {
    public class StageExit : MonoBehaviour {
        [SerializeField] private StageType stageType;
        [SerializeField] private Sprite[] sprites;
        private SpriteRenderer _spriteRenderer;
        private LevelManager _levelManager;
        private bool _isExitInUse;

        // Start is called before the first frame update
        void Start() {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
            _isExitInUse = false;
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag("Player")) {
                _isExitInUse = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (other.CompareTag("Player")) {
                _isExitInUse = false;
            }
        }

        // Update is called once per frame
        void Update() {
            UpdateSprite();
        }

        private void UpdateSprite() {
            switch (stageType) {
                case StageType.Unset:
                    _spriteRenderer.sprite = sprites[0];
                    break;

                case StageType.Boss:
                    _spriteRenderer.sprite = sprites[1];
                    break;
                
                case StageType.Spawn:
                    _spriteRenderer.sprite = sprites[2];
                    break;
                
                case StageType.Shop:
                    _spriteRenderer.sprite = sprites[3];
                    break;
                
                case StageType.Rest:
                    _spriteRenderer.sprite = sprites[4];
                    break;
                
                case StageType.Sirracha:
                    _spriteRenderer.sprite = sprites[5];
                    break;
                
                case StageType.Medium:
                    _spriteRenderer.sprite = sprites[6];
                    break;
                
                case StageType.Hard:
                    _spriteRenderer.sprite = sprites[7];
                    break;
                
                case StageType.Easy:
                    _spriteRenderer.sprite = sprites[8];
                    break;
                
                case StageType.Survival:
                    _spriteRenderer.sprite = sprites[9];
                    break;
            }
        }

        public void SetStageType(StageType type) {
            stageType = type;
        }
        
        public StageType GetStageType() {
            return stageType;
        }

        public bool GetIsExitInUse() {
            return _isExitInUse;
        }
    }
}