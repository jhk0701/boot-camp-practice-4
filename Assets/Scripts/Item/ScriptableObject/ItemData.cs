using UnityEngine;
using System;

public enum ItemType
{
    Resource,
    Equipable,
    Consumable,
}

public enum ConsumableType
{
    Health,
    Hunger,
}

[Serializable]
public class ItemDataConsumable
{
    public StatType consumableType;
    public float value;
}


[CreateAssetMenu(fileName = "ItemData",menuName = "boot-camp-practice-3/ItemData")]
public class ItemData : ScriptableObject
{
    public int id;
    public string title;
    public string description;
    public ItemType type;
    public Sprite icon;
    

    [Header("Stacking")]
    public bool canStack; // 여러개 소지할 수 있는지
    public int maxStackAmount; // 최대 소지수

    [Header("Consumable")]
    public ItemDataConsumable[] consumables; // 섭취 시 효과

    public GameObject dropPrefab;
    public GameObject equipPrefab;
}