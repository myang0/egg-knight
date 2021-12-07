using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverySpot : MonoBehaviour
{
    void Start()
    {
        SpriteRenderer _sr = GetComponent<SpriteRenderer>();
        var color = _sr.color;
        _sr.color = new Color(color.r, color.g, color.b, 0);
    }

}
