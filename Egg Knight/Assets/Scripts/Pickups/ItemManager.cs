using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemManager : MonoBehaviour {
    [SerializeField] private BaseItem placeholderItem;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public BaseItem SpawnItem(Vector3 spawnPos) {
        // To-do:
        Vector3 newPos = new Vector3(spawnPos.x, spawnPos.y, ZcoordinateConsts.Pickup);
        return Instantiate(placeholderItem, newPos, Quaternion.identity);
    }
}
