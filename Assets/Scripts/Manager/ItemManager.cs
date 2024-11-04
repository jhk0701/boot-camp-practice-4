using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    [SerializeField] ItemData[] items;
    [SerializeField] GameObject[] itemPrefabs;

    public Dictionary<int, GameObject> itemMap = new Dictionary<int, GameObject>();

    void Start()
    {
        for (int i = 0; i < items.Length; i++)
        {
            itemMap.Add(items[i].id, itemPrefabs[items[i].id]);
        }
    }
}