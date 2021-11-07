using System;
using System.Collections;
using System.Collections.Generic;
using Stage;
using UnityEngine;

public class TutorialRolling : StateMachineBehaviour
{
    private TutorialManager manager;
    private TutorialRoom tutRoom;
    private bool isEnemySpawned;
    private GameObject healingYolk;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        manager = GameObject.FindGameObjectWithTag("TutorialManager").GetComponent<TutorialManager>();
        tutRoom = manager.TutorialRooms[1];
        tutRoom.OnRoomEnter += StartDialogue;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (manager.forkItem == null) {
            animator.SetTrigger("NextRoom");
        }

        if (healingYolk == null) {
            healingYolk = Instantiate(manager.healingYolk, tutRoom.EntitySpawnPosTransform.position,
                Quaternion.identity);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (var dt in tutRoom.deadTrees) {
            dt.SetInvulnerability(false);
        }
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().Heal(100);
        Fungus.Flowchart.BroadcastFungusMessage("FinishRolling");
        tutRoom.OnRoomEnter = null;
    }

    private void StartDialogue(object sender, EventArgs e) {
        Fungus.Flowchart.BroadcastFungusMessage("StartRolling");
    }
}
