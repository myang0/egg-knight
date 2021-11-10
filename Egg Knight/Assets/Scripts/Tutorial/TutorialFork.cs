using System;
using System.Collections;
using System.Collections.Generic;
using Stage;
using UnityEngine;

public class TutorialFork : StateMachineBehaviour
{
    private TutorialManager manager;
    private StageManager stage;
    private TutorialRoom tutRoom;
    private bool isEnemySpawned;
    private bool isDialoguePlayed;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        manager = GameObject.FindGameObjectWithTag("TutorialManager").GetComponent<TutorialManager>();
        stage = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>().GetCurrentStage();
        tutRoom = manager.TutorialRooms[2];
        tutRoom.OnRoomEnter += StartDialogue;
        PlayerControls.On2Press += SpawnEnemy;
        TutorialManager.FsmEventHandler += ShowHelpText;
    }

    private void ShowHelpText(object sender, EventArgs e) {
        manager.wcText.SetText("Press 2 to swap to your fork! Defeat the strawberry!", 0);
    }

    private void SpawnEnemy(object sender, EventArgs e) {
        if (!isEnemySpawned && tutRoom.isPlayerInside) {
            SpawnParachute spawnParachute = Instantiate(manager.spawnParachute,
                tutRoom.EntitySpawnPosTransform.position, Quaternion.identity);
            spawnParachute.spawnSpecificEnemy = spawnParachute.lv1Strawberry;
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
        Fungus.Flowchart.BroadcastFungusMessage("FinishFork");
        tutRoom.OnRoomEnter = null;
        TutorialManager.FsmEventHandler -= ShowHelpText;
        manager.wcText.ResetText();
        PlayerControls.On2Press -= SpawnEnemy;
    }

    private void StartDialogue(object sender, EventArgs e) {
        if (isDialoguePlayed) return;
        Fungus.Flowchart.BroadcastFungusMessage("StartFork");
        isDialoguePlayed = true;
    }
}
