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
     
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        manager = GameObject.FindGameObjectWithTag("TutorialManager").GetComponent<TutorialManager>();
        stage = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>().GetCurrentStage();
        tutRoom = manager.TutorialRooms[0];
        TutorialManager.FsmEventHandler += SpawnEnemy;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isEnemySpawned && stage.enemyCount == 0) {
            animator.SetTrigger("NextRoom");
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (var dt in tutRoom.deadTrees) {
            dt.SetInvulnerability(false);
        }
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().Heal(100);
        Fungus.Flowchart.BroadcastFungusMessage("FinishKnife");
        TutorialManager.FsmEventHandler -= SpawnEnemy;
        tutRoom.OnRoomEnter = null;
    }

    public void SpawnEnemy(object sender, EventArgs e) {
        SpawnParachute spawnParachute = Instantiate(manager.spawnParachute,
            tutRoom.EntitySpawnPosTransform.position, Quaternion.identity);
        spawnParachute.spawnSpecificEnemy = spawnParachute.lv1EggGuard;
        stage.enemyCount++;
        isEnemySpawned = true;
    }
}
