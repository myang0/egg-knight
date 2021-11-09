using System;
using System.Collections;
using System.Collections.Generic;
using Stage;
using UnityEngine;

public class TutorialNotStarted : StateMachineBehaviour
{
    private StageManager stage;
    private TutorialManager manager;
    private TutorialRoom tutRoom;
    private bool _isDialogueTriggered;
     
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        manager = GameObject.FindGameObjectWithTag("TutorialManager").GetComponent<TutorialManager>();
        stage = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>().GetCurrentStage();
        tutRoom = manager.TutorialRooms[0];
        tutRoom.OnRoomEnter += StartDialogue;
    }

    private void StartDialogue(object sender, EventArgs e) {
        if (!_isDialogueTriggered) {
            Fungus.Flowchart.BroadcastFungusMessage("StartKnife");
            _isDialogueTriggered = true;
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_isDialogueTriggered) animator.SetTrigger("NextRoom");
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        tutRoom.OnRoomEnter -= StartDialogue;
        animator.ResetTrigger("NextRoom");
    }
}
