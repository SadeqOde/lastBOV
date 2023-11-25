using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttributeManager : MonoBehaviour
{
    public PlayerData playerData;

    public int currentHealth;
    public int maxHealth;
    public int attack;

    public Slider healthSlider;

    public GameObject deathEffect;

    public bool enemy;

    private void Update()
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }


     }

    public void DealDamage(GameObject target)
    {
        AttributeManager atm = target.GetComponent<AttributeManager>();

        if(atm != null)
        {
            atm.TakeDamage(attack);
        }
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;

        currentHealth = Mathf.Min(currentHealth, maxHealth);
    }

    public void LevelUp()
    {
        if (!enemy)
        {
            playerData.level += 1;
            playerData.maxHealth *= 2;
            currentHealth = playerData.maxHealth;
        }
    }

    private void Die()
    {
        if (enemy)
        {
            GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
            FindObjectOfType<PlayerMovement>().energy += 1;
            Destroy(gameObject);
        }
        else
        {
            GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        }
    }

}
