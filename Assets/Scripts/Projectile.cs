using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public int damageMultiplier;

    private AttributeManager attributeManager;

    void Start()
    {
        attributeManager = FindObjectOfType<PlayerMovement>().gameObject.GetComponent<AttributeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Move the projectile in forward direction
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<AttributeManager>().TakeDamage(attributeManager.attack * damageMultiplier);
            //effect
            Destroy(this.gameObject);
        }
    }
}
