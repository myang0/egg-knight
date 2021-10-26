using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemManager : MonoBehaviour {
    [SerializeField] private List<BaseItem> _items;
    [SerializeField] private List<BaseItem> _cursedItems;
    [SerializeField] private List<BaseItem> _healingItems;
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
    
    public BaseItem SpawnCursedItem() {
        Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
        Vector3 newPos = new Vector3(playerPos.x, playerPos.y, ZcoordinateConsts.Pickup);

        int randomItemIndex = Random.Range(0, _cursedItems.Count);

        BaseItem newItem = Instantiate(_cursedItems[randomItemIndex], newPos, Quaternion.identity);
        _cursedItems.Remove(_cursedItems[randomItemIndex]);
        return newItem;
    }

    public BaseItem GetRandomItem() {
        return _items[Random.Range(0, _items.Count)];
    }
    
    public BaseItem GetRandomHealingItem() {
        return _healingItems[Random.Range(0, _healingItems.Count)];
    }
}
