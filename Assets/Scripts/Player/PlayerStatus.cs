using System;
using UnityEngine;

public class PlayerStatus : MonoBehaviour, IDamagable
{
    public PlayerStat health;    
    public PlayerStat stamina;
    public PlayerStat mana;

    [SerializeField] float manaRecoverAmount = 1f;

    public event Action OnPlayerDamaged;


    void Update()
    {
        RecoverMana(manaRecoverAmount * Time.deltaTime);
    }


    public void TakeDamage(float amount)
    {
        health.Subtract(amount);
        OnPlayerDamaged?.Invoke();
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }
    
    public void Eat(float amount)
    {
        stamina.Add(amount);
    }

    public void RecoverMana(float amount)
    {
        mana.Add(amount);
    }

    public bool UseMana(float amount)
    {
        if(mana.Value - amount >= 0f)
        {
            mana.Subtract(amount);
            return true;
        }
        else
            return false;
    }

}