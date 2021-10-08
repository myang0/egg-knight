using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

namespace Stage
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private StageManager currentStage;
        [SerializeField] private List<StageManager> level1Stages;
        [SerializeField] private List<StageManager> level1Shops;
        [SerializeField] private StageManager level1Sirracha;
        [SerializeField] private StageManager level1Rest;
        [SerializeField] private List<StageManager> level2Stages;
        [SerializeField] private List<StageManager> level2Shops;
        [SerializeField] private StageManager level2Sirracha;
        [SerializeField] private StageManager level2Rest;
        [SerializeField] private List<StageManager> level3Stages;
        [SerializeField] private List<StageManager> level3Shops;
        [SerializeField] private StageManager level3Sirracha;
        [SerializeField] private StageManager level3Rest;
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private int stagesCleared;
        [SerializeField] private int restRate;
        [SerializeField] private int sirrachaRate;
        [SerializeField] private int shopRate;
        private int _level = 1;
        private const int ShopsPerLevel = 3;
        private GameObject _player;

        void Start() {
            StartAsserts();
            
            _player = GameObject.FindGameObjectWithTag("Player");
            restRate = 0;
            sirrachaRate = 0;
            shopRate = 0;
        }

        // Selects next stage/level based on current stage and the StageType param
        // Moves player to next stage, initiates stage, and removes stage from list
        public void NextStage(StageType stageType) {
            StageManager stage;

            switch (stageType) {
                case StageType.Spawn:
                    _level++;
                    stagesCleared = 0;
                
                    // First stage of list will always be SPAWN
                    stage = _level == 2 ? level2Stages[0] : level3Stages[0];
                    
                    break;
                
                case StageType.Boss:
                    stage = LoadBossStage();
                    break;
                
                case StageType.Shop:
                    stage = LoadShopStage();
                    break;
                
                case StageType.Sirracha:
                    stage = LoadSirrachaStage();
                    break;
                
                case StageType.Rest:
                    stage = LoadRestStage();
                    break;
                
                default:
                    stage = LoadRegularStage();
                    break;
            }
            
            stage.SetStageType(stageType);
            SetActiveStage(stage);
        }

        private StageManager LoadRegularStage() {
            StageManager stage;
            switch (_level) {
                case 1:
                    stage = level1Stages[Random.Range(0, level1Stages.Count - 1)];
                    Assert.IsTrue(level1Stages.Remove(stage));
                    break;
                case 2:
                    stage = level2Stages[Random.Range(0, level2Stages.Count - 1)];
                    Assert.IsTrue(level2Stages.Remove(stage));
                    break;
                default:
                    stage = level3Stages[Random.Range(0, level3Stages.Count - 1)];
                    Assert.IsTrue(level3Stages.Remove(stage));
                    break;
            }
            return stage;
        }

        private StageManager LoadRestStage() {
            StageManager stage;
            switch (_level) {
                case 1:
                    stage = level1Rest;
                    level1Rest = null;
                    break;
                case 2:
                    stage = level2Rest;
                    level2Rest = null;
                    break;
                default:
                    stage = level3Rest;
                    level3Rest = null;
                    break;
            }
            return stage;
        }

        private StageManager LoadSirrachaStage() {
            StageManager stage;
            switch (_level) {
                case 1:
                    stage = level1Sirracha;
                    level1Sirracha = null;
                    break;
                case 2:
                    stage = level2Sirracha;
                    level2Sirracha = null;
                    break;
                default:
                    stage = level3Sirracha;
                    level3Sirracha = null;
                    break;
            }
            return stage;
        }

        private StageManager LoadShopStage() {
            StageManager stage;
            switch (_level) {
                case 1:
                    stage = level1Shops[Random.Range(0, level1Shops.Count)];
                    Assert.IsTrue(level1Shops.Remove(stage));
                    break;
                case 2:
                    stage = level2Shops[Random.Range(0, level2Shops.Count)];
                    Assert.IsTrue(level2Shops.Remove(stage));
                    break;
                default:
                    stage = level3Shops[Random.Range(0, level3Shops.Count)];
                    Assert.IsTrue(level3Shops.Remove(stage));
                    break;
            }
            return stage;
        }

        private StageManager LoadBossStage() {
            StageManager stage;
            switch (_level) {
                case 1:
                    stage = level1Stages[level1Stages.Count - 1];
                    Assert.IsTrue(level1Stages.Remove(stage));
                    break;
                case 2:
                    stage = level2Stages[level2Stages.Count - 1];
                    Assert.IsTrue(level2Stages.Remove(stage));
                    break;
                default:
                    stage = level3Stages[level3Stages.Count - 1];
                    Assert.IsTrue(level3Stages.Remove(stage));
                    break;
            }
            return stage;
        }

        private void SetActiveStage(StageManager activeStage) {
            switch (_level) {
                case 1: {
                    foreach (StageManager stage in level1Stages) {
                        stage.SetStageStatus(StageStatus.Inactive);
                    }
                    break;
                }
                case 2: {
                    foreach (StageManager stage in level2Stages) {
                        stage.SetStageStatus(StageStatus.Inactive);
                    }
                    break;
                }
                case 3: {
                    foreach (StageManager stage in level3Stages) {
                        stage.SetStageStatus(StageStatus.Inactive);
                    }
                    break;
                }
            }
            
            Vector3 spawnPos = activeStage.GetSpawnPosition();
            Vector3 newPos = new Vector3(spawnPos.x, spawnPos.y, 2);
            _player.transform.position = newPos;
            activeStage.SetStageStatus(StageStatus.Active);
            virtualCamera.GetComponent<CinemachineConfiner>().m_BoundingVolume = activeStage.GetCameraBoundary();
            currentStage = activeStage;
        }

        private void StartAsserts() {
            Assert.IsNotNull(virtualCamera);
            Assert.IsTrue(level1Stages.Count > 0);
            Assert.IsTrue(level1Shops.Count == ShopsPerLevel);
            Assert.IsNotNull(level1Sirracha);
            Assert.IsNotNull(level1Rest);
            Assert.IsTrue(level2Stages.Count > 0);
            // Assert.IsTrue(level2Shops.Count == ShopsPerLevel);
            // Assert.IsNotNull(level2Sirracha);
            // Assert.IsNotNull(level2Rest);
            // Assert.IsTrue(level3Stages.Count > 0);
            // Assert.IsTrue(level3Shops.Count == ShopsPerLevel);
            // Assert.IsNotNull(level3Sirracha);
            // Assert.IsNotNull(level3Rest);
            Assert.IsTrue(_level > 0 && _level < 4);
        }
        
        public bool GetShopSpawn() {
            int chance = Random.Range(1, 101);

            switch (_level) {
                case 1:
                    if (level1Shops.Count == 0) return false;
                    chance = chance * 3 / level1Shops.Count;
                    break;
                case 2:
                    if (level2Shops.Count == 0) return false;
                    chance = chance * 3 / level2Shops.Count;
                    break;
                case 3:
                    if (level3Shops.Count == 0) return false;
                    chance = chance * 3 / level3Shops.Count;
                    break;
            }
            
            if (chance < shopRate) {
                shopRate = 0;
                return true;
            }
            return false;
        }
    
        public bool GetSirrachaSpawn() {
            switch (_level) {
                case 1 when level1Sirracha == null:
                case 2 when level1Sirracha == null:
                case 3 when level3Sirracha == null:
                    return false;
            }
            
            int chance = Random.Range(1, 101);
            if (chance < sirrachaRate) {
                sirrachaRate = 0;
                return true;
            }
            return false;
        }
    
        public bool GetRestSpawn() {
            switch (_level) {
                case 1 when level1Rest == null:
                case 2 when level2Rest == null:
                case 3 when level3Rest == null:
                    return false;
            }
            
            int chance = Random.Range(1, 101);
            if (chance < restRate) {
                restRate = 0;
                return true;
            }
            return false;
        }
    
        public void IncrementRest(int value) {
            restRate += value;
            if (restRate > 100) {
                restRate = 100;
            }
        }

        public void IncrementSirracha(int value) {
            sirrachaRate += value;
            if (sirrachaRate > 100) {
                sirrachaRate = 100;
            }
        }

        public void IncrementShop(int value) {
            shopRate += value;
            if (shopRate > 100) {
                shopRate = 100;
            }
        }

        public int GetLevel() {
            return _level;
        }
        
        public int GetStagesCleared() {
            return stagesCleared;
        }

        public void IncrementStagesCleared() {
            stagesCleared++;
        }

        public StageManager GetCurrentStage() {
            return currentStage;
        }
    }
}