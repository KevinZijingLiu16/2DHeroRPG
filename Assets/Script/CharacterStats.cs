using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public Stats strength;
    public Stats damage;
    public Stats maxHealth;
   [SerializeField] private int currentHealth;





   protected virtual void Start()
    {
        currentHealth = maxHealth.GetValue();


        
    }

    public virtual void DoDamge(CharacterStats _targetStats)
    {

        int totalDamage = damage.GetValue() + strength.GetValue();
        _targetStats.TakeDamage(totalDamage);
    }

    public virtual void TakeDamage(int _damage)
    {
        currentHealth -= _damage;
        Debug.Log(transform.name + " takes " + _damage + " damage.");

        if (currentHealth <= 0)
        {
            Die();
        }
       
    }

    public virtual void Die()
    {
        //throw new NotImplementedException();
    }
}
