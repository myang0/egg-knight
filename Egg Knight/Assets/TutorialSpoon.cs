using System;
using System.Collections;
using System.Collections.Generic;
using Stage;
using UnityEngine;

public class TutorialSpoon : StateMachineBehaviour
{
    private TutorialManager manager;
    private StageManager stage;
    private TutorialRoom tutRoom;
    private bool isEnemySpawned;
    private GameObject healingYolk;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        manager = GameObject.FindGameObjectWithTag("TutorialManager").GetComponent<TutorialManager>();
        stage = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>().GetCurrentStage();
        tutRoom = manager.TutorialRooms[3];
        tutRoom.OnRoomEnter += StartDialogue;
        PlayerControls.On3Press += SpawnEnemy;
    }

    private void SpawnEnemy(object sender, EventArgs e) {
        if (!isEnemySpawned) {
            SpawnParachute spawnParachute = Instantiate(manager.spawnParachute,
                tutRoom.EntitySpawnPosTransform.position, Quaternion.identity);
            spawnParachute.spawnSpecificEnemy = spawnParachute.lv1Raspberry;
            stage.enemyCount++;
            isEnemySpawned = true;
        }
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
        Fungus.Flowchart.BroadcastFungusMessage("FinishSpoon");
        tutRoom.OnRoomEnter = null;
        PlayerControls.On3Press -= SpawnEnemy;
    }

    private void StartDialogue(object sender, EventArgs e) {
        Fungus.Flowchart.BroadcastFungusMessage("StartSpoon");
    }
}
