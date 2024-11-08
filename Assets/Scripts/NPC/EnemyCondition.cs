using UnityEngine;
using System.Collections.Generic;

public class EnemyCondition : MonoBehaviour, IDamagable
{
    public bool IsDead { get; private set; } = false;

    [SerializeField] Stat[] enemyStats;
    public Dictionary<StatType, Stat> stats = new Dictionary<StatType, Stat>();

    void Start()
    {
        IsDead = false;
    }

    public void TakeDamage(float amount)
    {
        if (IsDead) return;

        stats[StatType.Health].Subtract(amount);
        if (stats[StatType.Health].Value <= 0f)
            Die();
    }

    void Die()
    {
        Debug.Log("is Dead");
        IsDead = true;
    }

}