using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
}
