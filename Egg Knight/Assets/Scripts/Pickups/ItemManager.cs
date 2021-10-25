using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemManager : MonoBehaviour {
    [SerializeField] private List<BaseItem> _items;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public BaseItem SpawnItem(Vector3 spawnPos) {
        Vector3 newPos = new Vector3(spawnPos.x, spawnPos.y, ZcoordinateConsts.Pickup);

        int randomItemIndex = Random.Range(0, _items.Count);

        return Instantiate(_items[randomItemIndex], newPos, Quaternion.identity);
    }

    public BaseItem SpawnItemByName(String itemName, Vector3 spawnPos) {
        Vector3 newPos = new Vector3(spawnPos.x, spawnPos.y, ZcoordinateConsts.Pickup);
        foreach (var item in _items) {
            if (item.DisplayName == itemName) {
                return Instantiate(item, newPos, Quaternion.identity);
            }
        }
        Debug.Log("Item of name: " + itemName + " not found in ItemManager");
        return null;
    }

    public BaseItem GetRandomItem() {
        return _items[Random.Range(0, _items.Count)];
    }
}
