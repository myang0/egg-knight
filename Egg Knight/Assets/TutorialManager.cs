using System;
using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;
using EventHandler = System.EventHandler;

public class TutorialManager : MonoBehaviour
{
    public List<TutorialRoom> TutorialRooms = new List<TutorialRoom>();
    public TutorialRoom CurrentRoom;
    private SpriteRenderer _sr;
    private WaveCounterText _waveCounterText;
    private bool isPlayerInRange;
    public SpawnParachute spawnParachute;
    public static EventHandler FsmEventHandler;

    // Start is called before the first frame update
    void Awake() {
        _sr = GetComponent<SpriteRenderer>();
        _waveCounterText = FindObjectOfType<WaveCounterText>();
        _sr.color = new Color(255, 255, 255, 0);
        foreach (var room in TutorialRooms) {
            room.OnRoomEnter += (sender, args) => {
                CurrentRoom = room;
                transform.position = room.EggithaTransform.position;
                ShowEggitha();
            };
            room.OnRoomExit += (sender, args) => {
                HideEggitha();
            };
        }
    }

    public void InvokeEvent() {
        FsmEventHandler?.Invoke(this, EventArgs.Empty);
    }

    private void HideEggitha() {
        StartCoroutine(DecreaseEggithaAlpha());
        StopCoroutine(IncreaseEggithaAlpha());
    }

    private void ShowEggitha() {
        StartCoroutine(IncreaseEggithaAlpha());
        StopCoroutine(DecreaseEggithaAlpha());
    }

    private IEnumerator IncreaseEggithaAlpha() {
        var color = _sr.color;
        float duration = 0.5f;
        float smoothness = 0.01f;
        float progress = 0;
        float increment = smoothness/duration;
        while(progress < 1)
        {
            _sr.color = Color.Lerp(color, new Color(color.r, color.g, color.b, 1), progress);
            progress += increment;
            yield return new WaitForSeconds(smoothness);
        }
    }
    
    private IEnumerator DecreaseEggithaAlpha() {
        var color = _sr.color;
        float duration = 0.5f;
        float smoothness = 0.01f;
        float progress = 0;
        float increment = smoothness/duration;
        while(progress < 1)
        {
            _sr.color = Color.Lerp(color, new Color(color.r, color.g, color.b, 0), progress);
            progress += increment;
            yield return new WaitForSeconds(smoothness);
        }
    }
}
