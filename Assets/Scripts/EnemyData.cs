using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyData : ScriptableObject
{

    public int currentHealth;
    public int maxHealth;
    public int attack;
    public float speed;
    public int level;

}
