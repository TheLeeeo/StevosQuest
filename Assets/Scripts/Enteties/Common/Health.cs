using UnityEngine;
using System.Collections.Generic;
using System.Collections.Specialized;
using System;


public class Health : MonoBehaviour
{
    public Immunity immunities;

    public int MaxHealth;
    public float CurrentHealth;

    public int MaxArmor;
    public float CurrentArmor;

    [SerializeField]
    private EntityController entityController;

    [SerializeField]
    private GameObject DamagedParticleEffectObject;

    public HealthBarController healthBar;
    [SerializeField]
    private Transform healthCanvasTransform;

    [SerializeField]
    private DropControllerBase lootDropController;

    private void Awake()
    {
        CurrentHealth = MaxHealth;
    }

    public Action OnDie;

    private EffectBase[] activeEffects = new EffectBase[21];


    public virtual int GiveEffect(Effect _effect)
    {
        int ID = (int)_effect.effectData.EffectType;

        if (false == immunities.IsImmuneTo(_effect.effectData.EffectType)) //Not immune to the effect;
        {
            if (null == activeEffects[ID]) //no effect of current type is active
            {
                activeEffects[ID] = Instantiate(_effect.effectData.EffectObject, transform).GetComponent<EffectBase>();
                activeEffects[ID].Activate(entityController, _effect.Duration, (int)_effect.EffectLevel);

                return 1; //effect was added
            }
            else //effect of current type is active
            {
                activeEffects[ID].UpdateTimeLeft(_effect.Duration);
                activeEffects[ID].UpdateEffectLevel((int)_effect.EffectLevel);
                return 2; //effect was updated
            }
        }

        return 0; //No effect was applied
    }

    public virtual void RemoveEffect(EffectEnum EffectID)
    {
        activeEffects[(int)EffectID]?.Deactivate(entityController);
        activeEffects[(int)EffectID] = null;
    }

    public void AddImmunity(EffectEnum effectType)
    {
        immunities.AddImmunity(effectType);

        RemoveEffect(effectType);
    }

    public void RemoveImmunity(EffectEnum effectType)
    {
        immunities.RemoveImmunity(effectType);
    }


    public bool HasMaxHealth()
    {
        return CurrentHealth == MaxHealth;
    }

    public bool HasMaxArmor()
    {
        return CurrentArmor == MaxArmor;
    }

    public virtual void Heal(int pointsToHeal)
    {
        CurrentHealth += pointsToHeal;

        if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;

            if(CurrentArmor >= MaxArmor)
            {
                healthBar.SetHealthBarActive(false);
                healthBar.SetArmorBarActive(false);
            }
            else
            {
                healthBar.SetHealth(CurrentHealth / MaxHealth);
            }         
        }
        else
        {
            healthBar.SetHealth(CurrentHealth / MaxHealth);
        }        
    }

    public virtual void AddArmor(int armorPoints)
    {
        CurrentArmor += armorPoints;

        if (CurrentArmor > MaxArmor)
        {
            CurrentArmor = MaxArmor;
        }

        healthBar.SetArmor(CurrentArmor / MaxArmor);
    }


    public virtual void Damage(int damage)
    {
        CurrentArmor -= damage;

        Instantiate(DamagedParticleEffectObject, transform);

        Instantiate(PrefabManager._instance.FloatingDamageTextPrefab, healthCanvasTransform.position, Quaternion.identity, LevelData.GetDamageTextCanvas()).GetComponent<DamageTextController>().Setup(damage); //Possible future pooling

        if (CurrentArmor <= 0)
        {
            CurrentHealth += CurrentArmor;
            CurrentArmor = 0;

            healthBar.SetArmorBarActive(false);

            Instantiate(DamagedParticleEffectObject, transform);

            healthBar.SetHealthBarActive(true);
            healthBar.SetHealth((float)CurrentHealth / MaxHealth);

            healthBar.SetArmor(0);

            if (CurrentHealth <= 0)
            {
                Die();
            }
        }
        else
        {
            healthBar.SetHealthBarActive(true);
            healthBar.SetHealth((float)CurrentHealth / MaxHealth);

            healthBar.SetArmorBarActive(true);
            healthBar.SetArmor((float)CurrentArmor / MaxArmor);
        }
    }


    public virtual void Damage(int damage, float armorPiercing)
    {
        float armorDamage = CurrentArmor / armorPiercing; //how much damage the armor can take
        float remainingDamage = damage - armorDamage;

        float damageTotal;

        if (remainingDamage > 0) //no armor left and damage ramaining
        {
            CurrentHealth -= remainingDamage;
            damageTotal = remainingDamage + CurrentArmor;

            CurrentArmor = 0;

            if (CurrentHealth <= 0)
            {
                Die();
            }
            else
            {
                healthBar.SetHealthBarActive(true);
                healthBar.SetHealth(CurrentHealth / MaxHealth);

                healthBar.SetArmor(0); //possibly need to turn it on in some edge case idk its late and i am tired
            }         

            //Instantiate(DamagedParticleEffectObject, transform);
        }
        else //armor >= 0
        {
            damageTotal = damage * armorPiercing;
            CurrentArmor -= damageTotal;

            healthBar.SetHealthBarActive(true);
            healthBar.SetHealth(CurrentHealth / MaxHealth);

            healthBar.SetArmorBarActive(true);
            healthBar.SetArmor(CurrentArmor / MaxArmor);
        }

        Instantiate(DamagedParticleEffectObject, transform);

        Instantiate(PrefabManager._instance.FloatingDamageTextPrefab, healthCanvasTransform.position, Quaternion.identity, LevelData.GetDamageTextCanvas()).GetComponent<DamageTextController>().Setup((int)damageTotal); //Possible future pooling

        /*if (CurrentArmor < 0)
        {
            CurrentHealth += (int)(CurrentArmor);
            CurrentArmor = 0;

            Instantiate(DamagedParticleEffectObject, transform);

            if (CurrentHealth <= 0)
            {
                Die();
            }
            else
            {
                healthBar.SetHealthBarActive(true);
                healthBar.SetHealth(CurrentHealth / MaxHealth);

                healthBar.SetArmor(0); //possibly need to turn it on in some edge case idk its late and i am tired
            }
        }
        else
        {
            healthBar.SetHealthBarActive(true);
            healthBar.SetHealth(CurrentHealth / MaxHealth);

            healthBar.SetArmorBarActive(true);
            healthBar.SetArmor(CurrentArmor / MaxArmor);
        }   */     
    }

    /// <summary>
    /// Change the max health by the given amount
    /// </summary>
    public void ChangeMaxHealth(int relavtiveHealthChange)
    {
        SetNewMaxHealth(relavtiveHealthChange + MaxHealth);
    }

    public void SetNewMaxHealth(int newMaxHealth)
    {
        if(newMaxHealth < MaxHealth)
        {
            MaxHealth = newMaxHealth;

            if(CurrentHealth > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }
        }
        else
        {
            int difference = newMaxHealth - MaxHealth;

            MaxHealth = newMaxHealth;

            Heal(difference);
        }

        healthBar.SetHealth(CurrentHealth / MaxHealth);
    }

    /// <summary>
    /// Change the max armor by the given amount
    /// </summary>
    public void ChangeMaxArmor(int relavtiveArmorChange)
    {
        SetNewMaxArmor(relavtiveArmorChange + MaxArmor);
    }

    public void SetNewMaxArmor(int newMaxArmor)
    {
        if (newMaxArmor < MaxArmor)
        {
            MaxArmor = newMaxArmor;

            if (CurrentArmor > MaxArmor)
            {
                CurrentArmor = MaxArmor;
            }
        }
        else
        {
            int difference = newMaxArmor - MaxArmor;

            MaxArmor = newMaxArmor;

            CurrentArmor += difference;
            healthBar.SetArmor(CurrentArmor / MaxArmor);
        }

        healthBar.SetArmor(CurrentArmor / MaxArmor);
    }

    private bool isAlive = true; //For if multiple bullets hit during the same frame the enemy would die multiple times and drop multiple drops which is kind of bad and unbalanced which is quite bad for a game.

    public virtual void Die()
    {
        if (isAlive)
        {
            entityController.deathEvent.Invoke();

            lootDropController.GenerateDrops();

            entityController.soundController.PlayDeathSound();

            Destroy(gameObject);

            isAlive = false;
        }        
    }
}
