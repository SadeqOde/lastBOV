using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    public int damageMultiplier;
    private AttributeManager attributeManager;

    // Start is called before the first frame update
    void Start()
    {
        attributeManager = FindObjectOfType<PlayerMovement>().gameObject.GetComponent<AttributeManager>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            other.GetComponent<AttributeManager>().TakeDamage(attributeManager.attack * damageMultiplier);
        }
    }
}
