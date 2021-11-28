using System;
using System.Collections;
using System.Collections.Generic;
using Stage;
using UnityEngine;

public class TutorialYolk : StateMachineBehaviour
{
    private TutorialManager manager;
    private TutorialRoom tutRoom;
    private bool isEnemySpawned;
    private GameObject dummy;
    private bool isDialoguePlayed;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        manager = GameObject.FindGameObjectWithTag("TutorialManager").GetComponent<TutorialManager>();
        tutRoom = manager.TutorialRooms[4];
        tutRoom.OnRoomEnter += StartDialogue;
        dummy = manager.trainingDummy;
        TutorialManager.FsmEventHandler += ShowHelpText;
    }

    private void ShowHelpText(object sender, EventArgs e) {
        manager.wcText.SetText("Right click to fire yolk! Destroy the suspicious target dummy!", 0);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (dummy == null) {
            animator.SetTrigger("NextRoom");
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (var dt in tutRoom.deadTrees) {
            dt.SetInvulnerability(false);
        }
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().Heal(100);
        Fungus.Flowchart.BroadcastFungusMessage("FinishYolk");
        TutorialManager.FsmEventHandler -= ShowHelpText;
        manager.wcText.ResetText();
        tutRoom.OnRoomEnter = null;
        manager.TutorialRooms[5].OnRoomEnter = null;
    }

    private void StartDialogue(object sender, EventArgs e) {
        if (isDialoguePlayed) return;
        Fungus.Flowchart.BroadcastFungusMessage("StartYolk");
        dummy.SetActive(true);
        isDialoguePlayed = true;
    }

    private void OnDestroy() {
        TutorialManager.FsmEventHandler -= ShowHelpText;
    }
}
