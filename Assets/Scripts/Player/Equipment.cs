using UnityEngine;
using UnityEngine.InputSystem;

public class Equipment : MonoBehaviour
{
    public Equip curEquip;
    public Transform equipParent;

    PlayerView view;
    PlayerStatus status;

    void Start()
    {
        view = GetComponent<PlayerView>();
        status = GetComponent<PlayerStatus>();

        Player p = GetComponent<Player>();
        p.inputController.OnAttackEvent += Attack;
    }

    public void EquipNew(ItemData data)
    {
        Unequip();
        curEquip = Instantiate(data.equipPrefab, equipParent).GetComponent<Equip>();
    }

    public void Unequip()
    {
        if(curEquip != null)
        {
            Destroy(curEquip.gameObject);
            curEquip = null;
        }
    }
    

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && 
            curEquip != null &&
            view.cursorIsLocked)
        {
            curEquip.OnAttackInput();   
        }
    }

    public void Attack()
    {
        if (curEquip != null && view.cursorIsLocked)
        {
            curEquip.OnAttackInput();   
        }
    }
}