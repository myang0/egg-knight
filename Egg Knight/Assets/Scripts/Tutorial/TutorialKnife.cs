using System;
using System.Collections;
using System.Collections.Generic;
using Stage;
using UnityEngine;

public class TutorialKnife : StateMachineBehaviour {
    private StageManager stage;
    private TutorialManager manager;
    private TutorialRoom tutRoom;
    private bool isEnemySpawned;
    private bool isDialoguePlayed;
     
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        manager = GameObject.FindGameObjectWithTag("TutorialManager").GetComponent<TutorialManager>();
        stage = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>().GetCurrentStage();
        tutRoom = manager.TutorialRooms[0];
        tutRoom.OnRoomEnter += StartDialogue;
        TutorialManager.FsmEventHandler += SpawnEnemy;
        manager.TutorialRooms[5].OnRoomEnter += SkipTutorialWarning;
    }

    private void SkipTutorialWarning(object sender, EventArgs e) {
        Fungus.Flowchart.BroadcastFungusMessage("SkipTutorial");
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isEnemySpawned && stage.enemyCount == 0) {
            animator.SetTrigger("NextRoom");
        }
        
        
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        manager.wcText.ResetText();
        foreach (var dt in tutRoom.deadTrees) {
            dt.SetInvulnerability(false);
        }
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().Heal(100);
        Fungus.Flowchart.BroadcastFungusMessage("FinishKnife");
        TutorialManager.FsmEventHandler -= SpawnEnemy;
        tutRoom.OnRoomEnter = null;
        manager.TutorialRooms[5].OnRoomEnter -= SkipTutorialWarning;
    }

    public void SpawnEnemy(object sender, EventArgs e) {
        if (!isEnemySpawned) {
            SpawnParachute spawnParachute = Instantiate(manager.spawnParachute,
                tutRoom.EntitySpawnPosTransform.position, Quaternion.identity);
            spawnParachute.spawnSpecificEnemy = spawnParachute.lv1EggGuard;
            stage.enemyCount++;
            isEnemySpawned = true;
            manager.wcText.SetText("Left click to attack! Defeat the Egg Guard!", 0);
        }
    }
    
    private void StartDialogue(object sender, EventArgs e) {
        if (!isDialoguePlayed) {
            Fungus.Flowchart.BroadcastFungusMessage("StartKnife");
            foreach (var dt in manager.TutorialRooms[5].deadTrees) {
                if (dt) dt.SetInvulnerability(true);
            }

            isDialoguePlayed = true;
        }
    }

    private void OnDestroy() {
        TutorialManager.FsmEventHandler -= SpawnEnemy;
    }
}
