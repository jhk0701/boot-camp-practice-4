using System;
using UnityEngine;

[Serializable]
public class StatusUsage
{
    public StatType type;
    public float usage;
}

public class Equip : MonoBehaviour
{
    public bool isAttacking = false;
    [SerializeField] protected float attackRate = 0.5f;

    // public float useStamina;
    public StatusUsage[] statusUsages;
    public float attackDistance = 5f;

    

    protected Animator animator;
    protected Camera cam;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        cam = Camera.main;
    }

    public virtual void OnAttackInput()
    {
        if (!isAttacking)
        {
            for (int i = 0; i < statusUsages.Length; i++)
            {
                if (!CharacterManager.Instance.Player.condition.IsUsable(statusUsages[i].type, statusUsages[i].usage))
                    return;
            }

            for (int i = 0; i < statusUsages.Length; i++)
            {
                CharacterManager.Instance.Player.condition.UseStat(statusUsages[i].type, statusUsages[i].usage);
            }

            isAttacking = true;
            animator.SetTrigger("Attack");
            Invoke("OnEnableAttack", attackRate);
        }
    }

    void OnEnableAttack()
    {
        isAttacking = false;
    }

    public virtual void OnHit()
    {

    }
}