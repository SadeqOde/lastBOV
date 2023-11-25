using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int damageMultiplier;
    public AttributeManager attributeManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<AttributeManager>().TakeDamage(attributeManager.attack * damageMultiplier);
        }
    }
}
