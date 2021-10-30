using System;
using System.Collections;
using System.Collections.Generic;
using Stage;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

public class EnemySpawnpoint : MonoBehaviour {
    public SpawnParachute spawnParachute;
    private SpriteRenderer _sr;

    void Awake() {
        _sr = GetComponent<SpriteRenderer>();
        var color = _sr.color;
        _sr.color = new Color(color.r, color.g, color.b, 0);
    }
    public void SpawnEnemy() {
        Vector3 oldPos = transform.position;
        Vector3 newPos = new Vector3(oldPos.x, oldPos.y, ZcoordinateConsts.Interactable);
        Instantiate(spawnParachute, newPos, Quaternion.identity);
    }
}
