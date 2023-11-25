using System.Collections;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    public float outerDetectionRadius = 5f;
    public float innerDetectionRadius = 2f;
    public float movementSpeed = 3f;
    public float attackCooldown;

    private bool isWalking;
    private bool isAttacking = false;
    private bool canAttack = true;

    private Transform player;
    private Animator anim;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= outerDetectionRadius && distanceToPlayer > innerDetectionRadius)
        {
            MoveTowardsPlayer();
        }
        else if (distanceToPlayer <= innerDetectionRadius)
        {
            if (!isAttacking && canAttack)
            {
                isWalking = false;
                Attack();
            }
            else
            {
                anim.SetBool("attack", false);
                anim.SetBool("walk", false);
                anim.SetBool("idle", true);
            }
        }
        else
        {
            isWalking = false;
            anim.SetBool("idle", true);
        }
    }

    private void Attack()
    {
        canAttack = false;
        isAttacking = true;
        anim.SetBool("walk", false);
        anim.SetBool("idle", false);
        anim.SetBool("attack", true);
    }

    void MoveTowardsPlayer()
    {
        isWalking = true;
        anim.SetBool("idle", false);
        anim.SetBool("attack", false);

        anim.SetBool("walk", true);
        Vector3 direction = (player.position - transform.position).normalized;
        transform.Translate(direction * movementSpeed * Time.deltaTime, Space.World);
        transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
    }

    //Called at the end of attack animation.
    public IEnumerator AfterAttack()
    {
        isAttacking = false;
        anim.SetBool("attack", false);
        if (isWalking)
        {
            anim.SetBool("walk", true);
        }
        else
        {
            anim.SetBool("idle", true);
        }

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, outerDetectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, innerDetectionRadius);
    }
}
