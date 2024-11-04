using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ItemSlot : MonoBehaviour
{
    [Header("Data")]
    public ItemData data;
    
    public int index;
    public int quantity;
    public bool equipped;

    [Header("UI")]
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI textCount;

    public event Action<int> OnSelectItem;


    // void Start()
    // {
    //     Clear();
    // }


    public void Set()
    {
        if(data == null)
        {
            Clear();
            return;
        }
        
        icon.sprite = data.icon;
        textCount.text = quantity.ToString();
    }

    void Clear()
    {
        icon.sprite = null;
        textCount.text = string.Empty;
    }
    
    public void Select()
    {
        OnSelectItem?.Invoke(index);
    }
}
