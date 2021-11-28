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
    private bool isDialoguePlayed;
    private PlayerHealth pHealth;
    private bool isHelpTextOn;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        manager = GameObject.FindGameObjectWithTag("TutorialManager").GetComponent<TutorialManager>();
        pHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        tutRoom = manager.TutorialRooms[1];
        tutRoom.OnRoomEnter += StartDialogue;
        TutorialManager.FsmEventHandler += EnableHelpText;
    }

    private void EnableHelpText(object sender, EventArgs e) {
        isHelpTextOn = true;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isHelpTextOn) {
            if (pHealth._currentHealth < pHealth._maxHealth) {
                manager.wcText.SetText("Pick up the healing yolk to heal or else the fork will disappear!", 0);
            }
            else {
                manager.wcText.SetText("Press SPACE while moving to roll! Roll over the stakes and pick up the fork!", 0);
            }
        }
        
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
        TutorialManager.FsmEventHandler -= EnableHelpText;
        manager.wcText.ResetText();
        tutRoom.OnRoomEnter = null;
    }

    private void StartDialogue(object sender, EventArgs e) {
        if (isDialoguePlayed) return;
        Fungus.Flowchart.BroadcastFungusMessage("StartRolling");
        isDialoguePlayed = true;
    }

    private void OnDestroy() {
        TutorialManager.FsmEventHandler -= EnableHelpText;
    }
}
