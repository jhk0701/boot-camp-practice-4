using UnityEngine;
using TMPro;
using System.Linq;

public class UIInventory : MonoBehaviour
{
    [SerializeField] ItemSlot[] slots;
    [SerializeField] GameObject panel;
    [SerializeField] Transform slotContainer;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI selectedItemName;
    [SerializeField] TextMeshProUGUI selectedItemDescription;
    [SerializeField] TextMeshProUGUI selectedItemStatName;
    [SerializeField] TextMeshProUGUI selectedItemStatValue;
    [SerializeField] GameObject useButton;
    [SerializeField] GameObject equipButton;
    [SerializeField] GameObject unequipButton;
    [SerializeField] GameObject dropButton;

    int selectedItemIndex;
    ItemData selectedItem;
    int curEquipIndex;

    
    PlayerCondition condition;


    void Start()
    {
        condition = CharacterManager.Instance.Player.condition;

        CharacterManager.Instance.Player.inputController.OnToggleInventoryEvent += Toggle;
        CharacterManager.Instance.Player.AddItem += AddItem;
    }

    public void Initialize()
    {
        slots = new ItemSlot[slotContainer.childCount];
        for (int i = 0; i < slotContainer.childCount; i++)
        {
            slots[i] = slotContainer.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].OnSelectItem += SelectItem;
            slots[i].Set();
        }

        ClearInfo();

        if (panel.activeInHierarchy)
            Toggle();
    }
    
    void Toggle()
    {
        panel.SetActive(!panel.activeInHierarchy);
    }

    void ClearInfo()
    {
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;
        
        useButton.SetActive(false);
        equipButton.SetActive(false);
        unequipButton.SetActive(false);
        dropButton.SetActive(false);
    }

    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
            slots[i].Set();
    }

    
    void AddItem(ItemData data)
    {
        // 축적 가능한 아이템인 경우
        // 기존에 있는지 확인하고 축적
        if(data.canStack)
        {
            ItemSlot slot = GetItemStacked(data);
            if(slot != null)
            {
                slot.quantity++;
                UpdateUI();

                return; 
            }
        }

        // 새로운 아이템 획득
        // 빈 슬롯 찾기
        ItemSlot empty = GetEmptyItem();

        if(empty != null)
        {
            empty.data = data;
            empty.quantity = 1;
            UpdateUI();

            return;
        }

        // 빈 슬롯이 없는 경우
        ThrowItem(data);
    }

    ItemSlot GetItemStacked(ItemData data)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].data == data)
            {
               return slots[i]; 
            }
        }
        return null;
    }

    ItemSlot GetEmptyItem()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].data == null)
            {
               return slots[i]; 
            }
        }
        return null;
    }

    void ThrowItem(ItemData data)
    {
        Transform player = CharacterManager.Instance.Player.transform;
        Vector3 position = player.position + Vector3.up + player.forward * 0.5f;
        Instantiate(ItemManager.Instance.itemMap[data.id], position, Quaternion.Euler(Vector3.one * Random.value * 360f));
    }

    void SelectItem(int index)
    {
        if(slots[index].data == null) return;

        selectedItemIndex = index;
        selectedItem = slots[index].data;

        selectedItemName.text = selectedItem.title;
        selectedItemDescription.text = selectedItem.description;

        
        selectedItemStatName.text = string.Empty; 
        selectedItemStatValue.text = string.Empty;

        for (int i = 0; i < selectedItem.consumables.Length; i++)
        {
            // TODO 스탯 출력
            selectedItemStatName.text += selectedItem.consumables[i].consumableType.ToString() + "\n"; 
            selectedItemStatValue.text += selectedItem.consumables[i].value.ToString() + "\n";
        }

        
        useButton.SetActive(selectedItem.type == ItemType.Consumable);
        equipButton.SetActive(selectedItem.type == ItemType.Equipable && !slots[index].equipped);
        unequipButton.SetActive(selectedItem.type == ItemType.Equipable && slots[index].equipped);

        dropButton.SetActive(true);
    }
    public void OnUseButton()
    {
        if(selectedItem.type == ItemType.Consumable)
        {
            for (int i = 0; i < selectedItem.consumables.Length; i++)
            {
                condition.RecoverStat(selectedItem.consumables[i].consumableType, selectedItem.consumables[i].value);
                // switch(selectedItem.consumables[i].consumableType)
                // {
                //     case ConsumableType.Health : 
                //         status.Heal(selectedItem.consumables[i].value);
                //         break;
                //     case ConsumableType.Hunger :
                //         status.Eat(selectedItem.consumables[i].value);
                //         break;
                // }
            }

            RemoveSelectedItem();
        }
    }

    public void OnDropButton()
    {
        ThrowItem(selectedItem);
        RemoveSelectedItem();
    }

    void RemoveSelectedItem()
    {
        slots[selectedItemIndex].quantity--;
        
        if (slots[selectedItemIndex].equipped)
            Unequip(selectedItemIndex);

        if(slots[selectedItemIndex].quantity <= 0)
        {
            slots[selectedItemIndex].data = null;

            selectedItem = null;
            selectedItemIndex = -1;

            ClearInfo();    
        }

        UpdateUI();
    }

    void Unequip(int index)
    {
        slots[index].equipped = false;
        CharacterManager.Instance.Player.equip.Unequip();
        UpdateUI();

        if(selectedItemIndex == index)
        {
            SelectItem(selectedItemIndex);
        }
    }

    
    public void OnEquipButton()
    {
        if(slots[curEquipIndex].equipped)
        {
            Unequip(curEquipIndex);
        }

        slots[selectedItemIndex].equipped = true;
        curEquipIndex = selectedItemIndex;

        CharacterManager.Instance.Player.equip.EquipNew(selectedItem);
        UpdateUI();

        SelectItem(selectedItemIndex);
    }

    public void OnUnequipButton()
    {
        Unequip(selectedItemIndex);
    }

}
