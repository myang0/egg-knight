using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorControl : MonoBehaviour
{
     [SerializeField] Texture2D cursor;
     
     void Start() {
            Cursor.SetCursor(cursor, Vector3.zero, CursorMode.ForceSoftware);
     }
}
