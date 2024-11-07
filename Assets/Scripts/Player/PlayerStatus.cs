using System;
using UnityEngine;
using System.Collections.Generic;

public enum StatType
{
    Health,
    Stamina,
    Mana,
}

public class PlayerStatus : MonoBehaviour, IDamagable
{
    public Dictionary<StatType, PlayerStat> stats = new Dictionary<StatType, PlayerStat>();
    [SerializeField] PlayerStat[] playerStats;

    // public PlayerStat health;    
    // public PlayerStat stamina;
    // public PlayerStat mana;

    [SerializeField] float manaRecoverAmount = 1f;

    public event Action OnPlayerDamaged;


    void Awake()
    {
        // 맵핑
        for (int i = 0; i < playerStats.Length; i++)
        {
            stats.Add(playerStats[i].type, playerStats[i]);
        }
    }


    void Update()
    {
        RecoverStat(StatType.Mana, manaRecoverAmount * Time.deltaTime);
    }


    public void TakeDamage(float amount)
    {
        // health.Subtract(amount);
        UseStat(StatType.Health, amount);
        OnPlayerDamaged?.Invoke();
    }

    public void RecoverStat(StatType type, float amount)
    {
        stats[type].Add(amount);
    }

    public void UseStat(StatType type, float amount)
    {
        stats[type].Subtract(amount);
    }

    public bool IsUsable(StatType type, float amount)
    {
        return stats[type].Value >= amount;
    }   

    public bool UseMana(float amount)
    {
        if (stats[StatType.Mana].Value - amount >= 0f)
        {
            stats[StatType.Mana].Subtract(amount);
            return true;
        }
        else
            return false;
    }

    public bool UseStamina(float amount)
    {
        if(stats[StatType.Stamina].Value - amount >= 0f)
        {
            stats[StatType.Stamina].Subtract(amount);
            return true;
        }
        else
            return false;
    }

}