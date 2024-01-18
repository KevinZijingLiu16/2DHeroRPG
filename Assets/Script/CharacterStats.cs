using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    private EntityFlash fx;


    [Header("Major Stats")]
    public Stats strength; // 1 point increase 1 damage and crit power by 1%
    public Stats agility; //1 point increase 1 evasion and crit chance by 1%
    public Stats intelligence;// 1 point increase 1 magic damage and magic crit power by 1%
    public Stats vitality; // 1 point increase 3 health 

    [Header("Offensive Stats")]
    public Stats damage;
    public Stats critChance;
    public Stats critPower;  // default 150% damage




    [Header("Defensive Stats")]
    public Stats maxHealth;
    public Stats armor;
    public Stats evasion;
    public Stats magicResistance;

    [Header("Magic Stats")]
    public Stats fireDamage;
    public Stats iceDamage;
    public Stats lightningDamage;


    public bool isIgnited; // does damage all the time
    public bool isChiiled; // decrease movement speed, decrese armor
    public bool isShocked; // reduce accuracy.

    [SerializeField] private float alimentDuration = 4f;
    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;


    private float igniteDamageCooldown = 0.3f;
    private float igniteDamageTimer;
    private int igniteDamage;
    private int shockDamage;
    [SerializeField] private GameObject shockStrikePrefab;





   public int currentHealth;

    public System.Action onHealthChanged;





   protected virtual void Start()
    {
        critPower.SetDefaultValue(150);
        currentHealth = GetMaxHealthValue();

        fx = GetComponent<EntityFlash>();
        
    }

    protected virtual void Update()
    {
       ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;

        igniteDamageTimer -= Time.deltaTime;
      
    
          if(ignitedTimer < 0)
          {
                isIgnited = false;
          }

          if(chilledTimer < 0)
        {
                isChiiled = false;
          }

          if(shockedTimer < 0)
        {
                isShocked = false;
          }
    
          if(igniteDamageTimer < 0 && isIgnited)
          {
             Debug.Log("Take Burn Damage" + igniteDamage);

            DecreaseHealthBy(igniteDamage);
            if(currentHealth <= 0)
            {
                   Die();
            }


                igniteDamageTimer = igniteDamageCooldown;
                
          }
    }

    public virtual void DoDamge(CharacterStats _targetStats)
    {
        if (TargetCanAvoidAttack(_targetStats))
        {
            return;
        }


        int totalDamage = damage.GetValue() + strength.GetValue();

        if(canCrit())
        {
            totalDamage = CalculateCritDamage(totalDamage);
            Debug.Log("Total Crit Damge is " + totalDamage);
        }

        totalDamage = CheckDamageArmor(_targetStats, totalDamage);
       // _targetStats.TakeDamage(totalDamage);

        DoMagicDamage(_targetStats);
    }


    public virtual void DoMagicDamage(CharacterStats _targetStats)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightningDamage = lightningDamage.GetValue();


        int totalMagicalDamage = _fireDamage + _iceDamage + _lightningDamage + intelligence.GetValue();

        totalMagicalDamage = CheckTargetResistance(_targetStats, totalMagicalDamage);

        _targetStats.TakeDamage(totalMagicalDamage);

        if(Mathf.Max(_fireDamage, _iceDamage, _lightningDamage) <= 0)
        {
            return;
        }



        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightningDamage;
        bool canApplyShock = _lightningDamage > _fireDamage && _lightningDamage > _iceDamage;


        while(!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
         if(UnityEngine.Random.value < 0.35f && _fireDamage > 0)
            {
                canApplyIgnite = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);

                Debug.Log("Apply Fire");
                return;
            }

         if (UnityEngine.Random.value < 0.5f && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                Debug.Log("Apply Ice");
                return;
            }
         if (UnityEngine.Random.value < 0.5f && _lightningDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                Debug.Log("Apply Lightning");
                return;
            }
        
        }

        if(canApplyIgnite)
        {
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * 0.2f));
        }

        if (canApplyShock)
        {
            _targetStats.SetupShockDamage(Mathf.RoundToInt(_lightningDamage * 0.1f));
        }

        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);



    }

    private static int CheckTargetResistance(CharacterStats _targetStats, int totalMagicalDamage)
    {
        totalMagicalDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
        return totalMagicalDamage;
    }

    public void ApplyAilments(bool _ignited, bool _chilled, bool _shocked)
    {
       bool canApplyIgnite = !isIgnited && !isChiiled && !isShocked;
        bool canApplyChill = !isIgnited && !isChiiled && !isShocked;
        bool canApplyShock = !isIgnited && !isChiiled;

        if (_ignited && canApplyIgnite)
        {
            isIgnited = _chilled;
            ignitedTimer = alimentDuration;

            fx.IgniteFxFor(alimentDuration);
        }

        if (_chilled && canApplyChill)
        {
            isChiiled = _chilled;
            chilledTimer = alimentDuration;

            float slowPercentage = 0.5f;
            GetComponent<Entity>().SlowEntityBy(slowPercentage, alimentDuration);

            fx.ChillFxFor(alimentDuration * 2f);
        }

        if (_shocked && canApplyShock)
        {
            if (!isShocked)
            {
                ApplyShock(_shocked);

            }

            else
            {
                if (GetComponent<Player>() != null)
                {
                    return;
                }

                HitNearestTargetWithStrike();

            }



            //find the nearest enemy and attack it.
            //initiate thunder strike

        }
        isIgnited = _ignited;
        isChiiled = _chilled;
        isShocked = _shocked;
    }

    public void ApplyShock(bool _shocked)
    {
        if(isShocked)
        {
            return;
        }


        isShocked = _shocked;
        shockedTimer = alimentDuration;

        fx.ShockFxFor(alimentDuration);
    }

    private void HitNearestTargetWithStrike()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25f);

        float closestDistance = Mathf.Infinity;

        Transform closestEnemy = null;
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > 1)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }

            }
            if (closestEnemy == null)
            {
                closestEnemy = transform;
            }
        }

        if (closestEnemy != null)
        {
            GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);
            newShockStrike.GetComponent<ShockStrikeController>().Setup(shockDamage, closestEnemy.GetComponent<CharacterStats>());
        }
    }

    public void SetupIgniteDamage(int _damage)
    {
        igniteDamage = _damage;
    }

    public void SetupShockDamage(int _damage)
    {
        shockDamage = _damage;
    }

    public virtual void TakeDamage(int _damage)
    {
        DecreaseHealthBy(_damage);
        Debug.Log(transform.name + " takes " + _damage + " damage.");

        if (currentHealth <= 0)
        {
            Die();
        }


       
    }


    protected virtual void DecreaseHealthBy(int _damage)
    {
        currentHealth -= _damage;

        if(onHealthChanged != null)
        {
            onHealthChanged();
        }
    }

    public virtual void Die()
    {
        //throw new NotImplementedException();
    }
    private bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (isShocked)
        {
            totalEvasion += 20;
        }
       

        if (UnityEngine.Random.Range(0, 100) < totalEvasion)
        {
            Debug.Log("Miss, attack avoided");
            return true;

        }

        return false;
    }
    private int CheckDamageArmor(CharacterStats _targetStats, int totalDamage)
    {
        if(_targetStats.isChiiled)
        {
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * 0.8f);

        }
        else
        {
            totalDamage -= _targetStats.armor.GetValue();
        
        }



        totalDamage -= _targetStats.armor.GetValue();
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    private bool canCrit()
    {
        int totalCritChance = critChance.GetValue() + agility.GetValue();
        if (UnityEngine.Random.Range(0, 100) <= totalCritChance)
        {
            return true;
        }
        return false;
    }

    private int CalculateCritDamage(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * 0.01f;

        Debug.Log("Total Crit Power %: " + totalCritPower);
        float critDamage = _damage * totalCritPower;
        Debug.Log("Crrit Damage before round: " + critDamage);
        return Mathf.RoundToInt(critDamage);
    }

    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }
}
