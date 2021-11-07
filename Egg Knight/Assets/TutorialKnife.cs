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
        Debug.Log("AAA");
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Enemy count starts at 0 because tech debt xd
        if (isEnemySpawned && stage.enemyCount < 0) {
            animator.SetTrigger("NextRoom");
            foreach (var dt in tutRoom.deadTrees) {
                dt.SetInvulnerability(false);
            }
            Fungus.Flowchart.BroadcastFungusMessage("FinishKnife");
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        TutorialManager.FsmEventHandler -= SpawnEnemy;
        tutRoom.OnRoomEnter = null;
    }

    public void SpawnEnemy(object sender, EventArgs e) {
        Debug.Log("TEST");
        SpawnParachute spawnParachute = Instantiate(manager.spawnParachute,
            tutRoom.EnemySpawnTransform.position, Quaternion.identity);
        spawnParachute.spawnSpecificEnemy = spawnParachute.lv1EggGuard;
        isEnemySpawned = true;
    }
}
