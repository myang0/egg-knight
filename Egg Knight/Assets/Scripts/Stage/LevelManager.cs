using System;
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
        [SerializeField] private int luckyItemRate;
        public bool isFirstShopVisited = false;
        public int numSirRachaVisits = 0;
        public bool hasPlayerTakenDamageCurrStage;
        public int level = 1;
        private const int ShopsPerLevel = 3;
        private GameObject _player;

        public static event EventHandler OnDialogueStart;
        public static event EventHandler OnDialogueEnd;

        private string _level1BossName = "Brigand Broccoli";
        public static event EventHandler<BossSpawnEventArgs> OnBroccoliFightBegin;
        public static event EventHandler OnBroccoliFightEnd;

        void Start() {
            StartAsserts();

            PlayerHealth.OnHealthDecrease += PlayerTookDamage;
            _player = GameObject.FindGameObjectWithTag("Player");
            restRate = 0;
            sirrachaRate = 0;
            shopRate = 0;
            UpdatePathing();
        }

        public void NextStage(StageType stageType) {
            StageManager stage;

            switch (stageType) {
                case StageType.Spawn:
                    isFirstShopVisited = false;
                    level++;
                    stagesCleared = 0;
                
                    // First stage of list will always be SPAWN
                    if (level == 2) {
                        stage = level2Stages[0];
                        level2Stages.Remove(stage);
                    }
                    else {
                        stage = level3Stages[0];
                        level3Stages.Remove(stage);
                    }
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
            SetStageActiveStatus(stage);
            MovePlayerToStage(stage);
            hasPlayerTakenDamageCurrStage = false;
            virtualCamera.GetComponent<CinemachineConfiner>().m_BoundingVolume = stage.GetCameraBoundary();
            currentStage = stage;
            UpdatePathing();
            stage.OnStageClear += IncrementRates;
        }

        private void UpdatePathing() {
            var graph = AstarPath.active.data.gridGraph;
            var stagePos = currentStage.transform.position;
            var colliderCenter = currentStage._camBoundary.center;
            graph.SetDimensions(Mathf.RoundToInt(currentStage._camBoundary.size.x), 
                Mathf.RoundToInt(currentStage._camBoundary.size.y), 1);
            graph.center = new Vector2(stagePos.x + colliderCenter.x, stagePos.y + colliderCenter.y);
            // Debug.Log("X: " + graph.center.x + ", Y: " + graph.center.y);
            // graph.center = currentStage.transform.position;
            AstarPath.active.Scan();
        }

        private void MovePlayerToStage(StageManager stage) {
            Vector3 spawnPos = stage.GetSpawnPosition();
            Vector3 newPos = new Vector3(spawnPos.x, spawnPos.y, ZcoordinateConsts.Character);
            _player.transform.position = newPos;
        }

        private void PlayerTookDamage(object sender, EventArgs e) {
            hasPlayerTakenDamageCurrStage = true;
        }

        private void IncrementRates(object sender, EventArgs e) {
            StageType currStageType = currentStage.GetStageType();
            if (currStageType == StageType.Spawn) return;
            
            stagesCleared++;
            restRate += 5;
            shopRate += 10;
            sirrachaRate += 1;

            switch (currStageType) {
                case StageType.Hard:
                    if (!hasPlayerTakenDamageCurrStage) {
                        luckyItemRate += 5;
                        sirrachaRate += 15; 
                    }

                    luckyItemRate += 3;
                    break;
                
                case StageType.Medium:
                    if (!hasPlayerTakenDamageCurrStage) {
                        luckyItemRate += 3;
                        sirrachaRate += 10; 
                    }
                    luckyItemRate += 2;
                    break;
                
                case StageType.Easy:
                    if (!hasPlayerTakenDamageCurrStage) {
                        luckyItemRate += 2;
                        sirrachaRate += 5; 
                    }
                    luckyItemRate += 1;
                    break;
                
                case StageType.Survival:
                    if (!hasPlayerTakenDamageCurrStage) {
                        luckyItemRate += 5;
                        sirrachaRate += 15; 
                    }
                    luckyItemRate += 4;
                    break;
                
                case StageType.Boss:
                    if (!hasPlayerTakenDamageCurrStage) {
                        luckyItemRate += 100;
                        sirrachaRate += 100;
                    }
                    luckyItemRate += 5;
                    sirrachaRate += 5;
                    break;
                }
        }

        private StageManager LoadRegularStage() {
            StageManager stage;
            switch (level) {
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
            switch (level) {
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
            switch (level) {
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
            switch (level) {
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
            switch (level) {
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

        private void SetStageActiveStatus(StageManager activeStage) {
            switch (level) {
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
            activeStage.SetStageStatus(StageStatus.Active);
        }

        private void StartAsserts() {
            Assert.IsNotNull(virtualCamera);
            Assert.IsTrue(level1Stages.Count > 0);
            Assert.IsTrue(level1Shops.Count == ShopsPerLevel);
            Assert.IsNotNull(level1Sirracha);
            Assert.IsNotNull(level1Rest);
            Assert.IsTrue(level2Stages.Count > 0);
            Assert.IsTrue(level2Shops.Count == ShopsPerLevel);
            Assert.IsNotNull(level2Sirracha);
            Assert.IsNotNull(level2Rest);
            // Assert.IsTrue(level3Stages.Count > 0);
            // Assert.IsTrue(level3Shops.Count == ShopsPerLevel);
            // Assert.IsNotNull(level3Sirracha);
            // Assert.IsNotNull(level3Rest);
            Assert.IsTrue(level > 0 && level < 4);
        }
        
        public bool GetShopSpawn() {
            int chance = Random.Range(1, 101);
            if (_player.GetComponent<PlayerWallet>().GetBalance() < 4) return false;

            switch (level) {
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
            switch (level) {
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
            if (_player.GetComponent<PlayerHealth>().CurrentHealth > 65) return false;
            
            switch (level) {
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

        public bool GetLuckyItemSpawn() {
            int chance = Random.Range(1, 101);
            if (chance < luckyItemRate) {
                luckyItemRate = 0;
                return true;
            }
            return false;
        }

        public int GetLevel() {
            return level;
        }
        
        public int GetStagesCleared() {
            return stagesCleared;
        }

        public StageManager GetCurrentStage() {
            return currentStage;
        }

        public void StartDialogue() {
            OnDialogueStart.Invoke(this, EventArgs.Empty);
        }

        public void EndDialogue() {
            OnDialogueEnd.Invoke(this, EventArgs.Empty);
        }

        public void BeginBroccoliFight() {
            GameObject broccoliObject = GameObject.Find("BrigandBroccoli");
            broccoliObject.GetComponent<Animator>().SetBool("IsActive", true);

            OnBroccoliFightBegin?.Invoke(this, new BossSpawnEventArgs(_level1BossName));
        }

        public void EndBroccoliFight() {

        }
    }
}