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
        [SerializeField] private List<StageManager> level2Stages;
        [SerializeField] private List<StageManager> level3Stages;
        [SerializeField] private CinemachineVirtualCamera virtualCamera;
        [SerializeField] private int stagesCleared;
        [SerializeField] private int restRate;
        [SerializeField] private int sirrachaRate;
        [SerializeField] private int shopRate;
        private int _level = 1;
        private GameObject _player;

        // Start is called before the first frame update
        void Start()
        {
            Assert.IsNotNull(virtualCamera);
            Assert.IsTrue(level1Stages.Count > 0);
            // Assert.IsTrue(level2Stages.Count > 0);
            // Assert.IsTrue(level3Stages.Count > 0);
            Assert.IsTrue(_level > 0 && _level < 4);
            
            _player = GameObject.FindGameObjectWithTag("Player");
            restRate = 0;
            sirrachaRate = 0;
            shopRate = 0;
        }

        // Update is called once per frame
        void Update()
        { }

        // Selects next stage/level based on current stage and the StageType param
        // Moves player to next stage, initiates stage, and removes stage from list
        public void NextStage(StageType stageType) {
            StageManager stage;
            
            // If next stage is SPAWN aka next level spawn
            if (stageType == StageType.Spawn) {
                _level++;
                stagesCleared = 0;
                
                // First stage of list will always be SPAWN
                stage = _level == 2 ? level2Stages[0] : level3Stages[0];
            } 
            // If next stage is BOSS
            else if (stageType == StageType.Boss) {
                
                //Last stage of list will always be BOSS
                if (_level == 1) {
                    stage = level1Stages[level1Stages.Count - 1];

                } else if (_level == 2) {
                    stage = level2Stages[level2Stages.Count - 1];

                } else {
                    stage = level3Stages[level3Stages.Count - 1];
                }
                
            } 
            // If next stage is regular/special stage
            else {
                if (_level == 1) {
                    int stageIndex = Random.Range(0, level1Stages.Count-1);
                    stage = level1Stages[stageIndex];

                } else if (_level == 2) {
                    int stageIndex = Random.Range(0, level2Stages.Count-1);
                    stage = level2Stages[stageIndex];
                
                } else {
                    int stageIndex = Random.Range(0, level3Stages.Count-1);
                    stage = level3Stages[stageIndex];
                }
            }
            
            stage.SetStageType(stageType);
            Vector3 spawnPos = stage.GetSpawnPosition();
            Vector3 newPos = new Vector3(spawnPos.x, spawnPos.y, 2);
            _player.transform.position = newPos;
            SetActiveStage(stage);
            
            // Remove stage from the list after moving player to that stage
            if (_level == 1) {
                Assert.IsTrue(level1Stages.Remove(stage));
            } else if (_level == 2) {
                Assert.IsTrue(level2Stages.Remove(stage));
            } else {
                Assert.IsTrue(level3Stages.Remove(stage));
            }
        }

        private void SetActiveStage(StageManager activeStage) {
            switch (_level) {
                case 1: {
                    foreach (StageManager stage in level1Stages) {
                        SetActiveStageHelper(stage, activeStage);
                    }
                    break;
                }
                case 2: {
                    foreach (StageManager stage in level2Stages) {
                        SetActiveStageHelper(stage, activeStage);
                    }
                    break;
                }
                case 3: {
                    foreach (StageManager stage in level3Stages) {
                        SetActiveStageHelper(stage, activeStage);
                    }
                    break;
                }
            }
        }

        private void SetActiveStageHelper(StageManager stage, StageManager activeStage) {
            if (stage != activeStage) {
                stage.SetStageStatus(StageStatus.Inactive);
            } else {
                stage.SetStageStatus(StageStatus.Active);
                virtualCamera.GetComponent<CinemachineConfiner>().m_BoundingVolume = activeStage.GetCameraBoundary();
                currentStage = activeStage;
            }
        }

        public bool GetShopSpawn() {
            int chance = Random.Range(1, 101);
            if (chance < shopRate) {
                shopRate = 0;
                return true;
            }
            return false;
        }
    
        public bool GetSirrachaSpawn() {
            int chance = Random.Range(1, 101);
            if (chance < sirrachaRate) {
                sirrachaRate = 0;
                return true;
            }
            return false;
        }
    
        public bool GetRestSpawn() {
            int chance = Random.Range(1, 101);
            if (chance < restRate) {
                restRate = 0;
                return true;
            }
            return false;
        }
    
        public void IncrementRest(int value) {
            restRate += value;
        }

        public void IncrementSirracha(int value) {
            sirrachaRate += value;
        }

        public void IncrementShop(int value) {
            shopRate += value;
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