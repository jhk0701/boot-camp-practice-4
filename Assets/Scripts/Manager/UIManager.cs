using UnityEngine;
using System;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] UIInventory uiInventoryPrefab;
    public UIInventory uiInventory;

    // UI State
    bool settingOpened = false;
    bool inventoryOpened = false;
    public event Action<bool> OnMouseLock;


    void Awake()
    {
        uiInventory = Instantiate(uiInventoryPrefab, transform);
        uiInventory.Initialize();
    }

    void Start()
    {
        PlayerInputController input = CharacterManager.Instance.Player.inputController;
        input.OnToggleInventoryEvent += () => 
        { 
            inventoryOpened = !inventoryOpened;
            CallMouseLock();
        };
        input.OnToggleSettingEvent += () => 
        { 
            settingOpened = !settingOpened;
            CallMouseLock();
        };
    }

    void CallMouseLock()
    {
        OnMouseLock?.Invoke(inventoryOpened || settingOpened);
    }
}
