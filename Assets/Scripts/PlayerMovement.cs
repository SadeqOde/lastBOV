using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerData playerData;
    private AttributeManager attributeManager;

    private FloatingJoystick joystick;
    private CameraFollow cameraFollow;

    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _speed = 5;
    [SerializeField] private float _turnSpeed = 360;
    private Vector3 _input;

    private Animator anim;

    public float maxComboDelay = 1f;
    private float lastClickedTime = 0f;
    private float nextFireTime = 0f;
    private int noOfClicks = 0;

    public bool canMove = true;
    public bool canAttack = true;
    public bool canUseCharged = false;
    public bool ultimateReady = false;

    public float chargeCooldown;
    public int energy;
    public int maxEnergy;

    public Transform shootingPoint;
    public GameObject[] projectiles;
    private int projectileCount;

    public GameObject gameOver;

    public bool melee = true;

    public bool onEditor = false;

    //for ultimate
    public GameObject mainCamera;

    private void Start()
    {
        anim = GetComponent<Animator>();
        joystick = FindObjectOfType<FloatingJoystick>();
        attributeManager = GetComponent<AttributeManager>();

        maxEnergy = playerData.maxEnergy;
        attributeManager.maxHealth = playerData.maxHealth;
        attributeManager.currentHealth = attributeManager.maxHealth;

        cameraFollow = FindObjectOfType<CameraFollow>();
        if (cameraFollow != null)
        {
            cameraFollow.playerTransform = transform;
        }
    }

    private void Update()
    {
        GatherInput();
        Look();

        if(onEditor)
        {
            HandleAttacks();
        }
        else
        {
            if (Time.time - lastClickedTime > maxComboDelay)
            {
                ResetAttack();
            }
        }

        if(energy >= maxEnergy)
        {
            energy = maxEnergy;
            ultimateReady = true;
        }
        else
        {
            ultimateReady = false;
        }

        attributeManager.maxHealth = playerData.maxHealth;
        playerData.currentHealth = attributeManager.currentHealth;
        playerData.energy = maxEnergy;

        if(attributeManager.currentHealth <= 0)
        {
            Time.timeScale = 0;
            gameOver.SetActive(true);
        }
        
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void GatherInput()
    {
        if (onEditor)
        {
            _input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        }
        else
        {
            _input = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
        }
    }

    private void Look()
    {
        if (_input == Vector3.zero) return;

        var rot = Quaternion.LookRotation(_input.ToIso(), Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, _turnSpeed * Time.deltaTime);
    }

    private void Move()
    {
        if (canMove)
        {
            Vector3 movement = transform.forward * _input.normalized.magnitude;

            if (movement.magnitude > 0)
            {
                anim.SetBool("Walk", true);
            }
            else
            {
                anim.SetBool("Walk", false);
            }

            _rb.MovePosition(transform.position + movement * _speed * Time.deltaTime);

            if (movement.magnitude == 0)
            {
                anim.SetBool("idle", true);
            }
            else
            {
                anim.SetBool("idle", false);
            }
        }
    }

    public void NormalAttack()
    {
        if (melee)
        {
            if (Time.time > nextFireTime && canAttack)
            {
                lastClickedTime = Time.time;

                Vector3 movement = transform.forward * _input.normalized.magnitude;

                if (movement.magnitude > 0)
                {
                    anim.SetBool("Walk", false);
                    if (noOfClicks == 0)
                    {
                        noOfClicks = 1;
                        anim.SetBool("hit1", true);
                    }
                    else if (noOfClicks == 1 && anim.GetCurrentAnimatorStateInfo(0).IsName("attack") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.15f)
                    {
                        noOfClicks = 2;
                        anim.SetBool("hit2", true);
                        anim.SetBool("hit1", false);
                    }
                    else if (noOfClicks == 2 && anim.GetCurrentAnimatorStateInfo(0).IsName("attack2") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.1f)
                    {
                        noOfClicks = 3;
                        anim.SetBool("hit3", true);
                        anim.SetBool("hit2", false);
                    }
                    else
                    {
                        ResetAttack();
                    }
                }
                else
                {
                    if (noOfClicks == 0)
                    {
                        noOfClicks = 1;
                        anim.SetBool("hit1", true);
                    }
                    else if (noOfClicks == 1 && anim.GetCurrentAnimatorStateInfo(0).IsName("attack") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.15f)
                    {
                        noOfClicks = 2;
                        anim.SetBool("hit2", true);
                        anim.SetBool("hit1", false);
                    }
                    else if (noOfClicks == 2 && anim.GetCurrentAnimatorStateInfo(0).IsName("attack2") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.1f)
                    {
                        noOfClicks = 3;
                        anim.SetBool("hit3", true);
                        anim.SetBool("hit2", false);
                    }
                    else
                    {
                        ResetAttack();
                    }
                }
            }
        }
        else
        {
            float zRotation = 0f;
            if (projectileCount == 0)
            {
                zRotation = -30f;
            }
            else if (projectileCount == 1)
            {
                zRotation = 30f;
            }

            if (projectileCount == 0)
            {
                Quaternion spawnRotation = shootingPoint.rotation * Quaternion.Euler(0f, 0f, zRotation);
                GameObject projectile = Instantiate(projectiles[projectileCount], shootingPoint.transform.position, spawnRotation);
                projectileCount++;
                DespawnProjectile(projectile);
            }
            else if (projectileCount == 1)
            {
                Quaternion spawnRotation = shootingPoint.rotation * Quaternion.Euler(0f, 0f, zRotation);
                GameObject projectile = Instantiate(projectiles[projectileCount], shootingPoint.transform.position, spawnRotation);
                projectileCount++;
                DespawnProjectile(projectile);
            }
            else if (projectileCount >= 2)
            {
                Quaternion spawnRotation = shootingPoint.rotation * Quaternion.Euler(0f, 0f, zRotation);
                GameObject projectile = Instantiate(projectiles[projectileCount], shootingPoint.transform.position, spawnRotation);
                projectileCount = 0;
                DespawnProjectile(projectile);
            }
        }
    }




    //For testing purposes
    private void HandleAttacks()
    {
        if (melee)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                ChargedAttack();
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                UltimateAttack();
            }

            if (Time.time - lastClickedTime > maxComboDelay)
            {
                ResetAttack();
            }

            if (Time.time > nextFireTime && Input.GetMouseButtonDown(0))
            {
                lastClickedTime = Time.time;

                Vector3 movement = transform.forward * _input.normalized.magnitude;

                if (movement.magnitude > 0)
                {
                    anim.SetBool("Walk", false);
                    if (noOfClicks == 0)
                    {
                        noOfClicks = 1;
                        anim.SetBool("hit1", true);
                    }
                    else if (noOfClicks == 1 && anim.GetCurrentAnimatorStateInfo(0).IsName("attack") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.15f)
                    {
                        noOfClicks = 2;
                        anim.SetBool("hit2", true);
                        anim.SetBool("hit1", false);
                    }
                    else if (noOfClicks == 2 && anim.GetCurrentAnimatorStateInfo(0).IsName("attack2") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.1f)
                    {
                        noOfClicks = 3;
                        anim.SetBool("hit3", true);
                        anim.SetBool("hit2", false);
                    }
                    else
                    {
                        ResetAttack();
                    }
                }
                else
                {
                    if (noOfClicks == 0)
                    {
                        noOfClicks = 1;
                        anim.SetBool("hit1", true);
                    }
                    else if (noOfClicks == 1 && anim.GetCurrentAnimatorStateInfo(0).IsName("attack") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.15f)
                    {
                        noOfClicks = 2;
                        anim.SetBool("hit2", true);
                        anim.SetBool("hit1", false);
                    }
                    else if (noOfClicks == 2 && anim.GetCurrentAnimatorStateInfo(0).IsName("attack2") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.1f)
                    {
                        noOfClicks = 3;
                        anim.SetBool("hit3", true);
                        anim.SetBool("hit2", false);
                    }
                    else
                    {
                        ResetAttack();
                    }
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                float zRotation = 0f;
                if (projectileCount == 0)
                {
                    zRotation = -30f;
                }
                else if (projectileCount == 1)
                {
                    zRotation = 30f;
                }

                if (projectileCount == 0)
                {
                    Quaternion spawnRotation = shootingPoint.rotation * Quaternion.Euler(0f, 0f, zRotation);
                    GameObject projectile = Instantiate(projectiles[projectileCount], shootingPoint.transform.position, spawnRotation);
                    projectileCount++;
                    DespawnProjectile(projectile);
                }
                else if (projectileCount == 1)
                {
                    Quaternion spawnRotation = shootingPoint.rotation * Quaternion.Euler(0f, 0f, zRotation);
                    GameObject projectile = Instantiate(projectiles[projectileCount], shootingPoint.transform.position, spawnRotation);
                    projectileCount++;
                    DespawnProjectile(projectile);
                }
                else if (projectileCount >= 2)
                {
                    Quaternion spawnRotation = shootingPoint.rotation * Quaternion.Euler(0f, 0f, zRotation);
                    GameObject projectile = Instantiate(projectiles[projectileCount], shootingPoint.transform.position, spawnRotation);
                    projectileCount = 0;
                    DespawnProjectile(projectile);
                }
            }
        }
    }

    private IEnumerator DespawnProjectile(GameObject projectile)
    {
        yield return new WaitForSeconds(3f);
        Destroy(projectile);
    }



    private void ResetAttack()
    {
        
        noOfClicks = 0;
        anim.SetBool("hit1", false);
        anim.SetBool("hit2", false);
        anim.SetBool("hit3", false);
    }

    public void ChargedAttack()
    {
        if(canUseCharged)
        {
            canMove = false;
            anim.SetBool("Walk", false);
            anim.SetBool("idle", false);
            ResetAttack();
            canAttack = false;
            anim.SetBool("charged", true);
            StartCoroutine(ChargedCooldown());
        }
    }

    private IEnumerator ChargedCooldown()
    {
        canUseCharged = false;
        yield return new WaitForSeconds(1.2f);
        anim.SetBool("charged", false);
        canMove = true;
        canAttack = true;
        yield return new WaitForSeconds(chargeCooldown);
        canUseCharged = true;
    }

    public void UltimateAttack()
    {
        if(ultimateReady)
        {
            canMove = false;
            mainCamera.SetActive(false);
            anim.SetBool("Walk", false);
            anim.SetBool("idle", false);
            ResetAttack();
            canAttack = false;
            canUseCharged = false;
            anim.SetBool("ultimate", true);
            StartCoroutine(UltimateCooldown());
        }
        

    }

    private IEnumerator UltimateCooldown()
    {
        ultimateReady = false;
        yield return new WaitForSeconds(1.5f);
        energy = 0;
        anim.SetBool("ultimate", false);
        canMove = true;
        canAttack = true;
        canUseCharged = true;
    }

    public void ShowCamera()
    {
        mainCamera.SetActive(true);
    }
}

public static class Helpers
{
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);
}
