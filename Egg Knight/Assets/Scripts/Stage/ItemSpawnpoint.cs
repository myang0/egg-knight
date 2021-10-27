using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnpoint : MonoBehaviour
{
    private void Awake() {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        var color = sr.color;
        sr.color = new Color(color.r, color.g, color.b, 0);
    }
}
