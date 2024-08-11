using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StandingZombie : MonoBehaviour
{
    [Header("Zombie Health and Swap")]
    private float zombieHealth = 100f;
    public float presentHealth;
    public float giveDamage = 5;

    [Header("Zombie things")]
    public NavMeshAgent zombieAgent;
    public LayerMask PlayerLayer;
    public Transform PlayerBody;
    public Camera AttackingRaycastArea;
    public Transform lookPoint;
    public HealthBar healthBar;


    [Header("Zombie Standing Var")]
    public float zombieSpeed;

    [Header("Zombie Attacking Var")]
    public float timeBtwAttack;
    bool previouslyAttacked;

    [Header("Zombie Animator")]
    public Animator anim;

    [Header("Zombie Mode/State")]
    public float visionRadius;
    public float attackingRadius;
    public bool playerInVisionRadius;
    public bool playerInAttackingRadius;

    [Header("Zombie Sound")]
    public AudioSource audioSource;
    public AudioClip death;

    private void Awake()
    {
        presentHealth = zombieHealth;
        zombieAgent = GetComponent<NavMeshAgent>();
        healthBar.GiveFullHealth(zombieHealth);
    }

    private void Update()
    {
        playerInVisionRadius = Physics.CheckSphere(transform.position, visionRadius, PlayerLayer);
        playerInAttackingRadius = Physics.CheckSphere(transform.position, attackingRadius, PlayerLayer);

        if (!playerInVisionRadius && !playerInAttackingRadius) Idle();
        if (playerInVisionRadius && !playerInAttackingRadius) PursuePlayer();
        if (playerInVisionRadius && playerInAttackingRadius) AttackPlayer();
    }

    private void Idle()
    {
        zombieAgent.SetDestination(transform.position);
        //animations
        anim.SetBool("Idle", true);
        anim.SetBool("Running", false);
    }

    private void PursuePlayer()
    {
        if (zombieAgent.SetDestination(PlayerBody.position))
        {
            //animations
            anim.SetBool("Idle", false);
            anim.SetBool("Running", true);
            anim.SetBool("Attacking", false);
        }
        else
        {
            //animations
            anim.SetBool("Idle", true);
            anim.SetBool("Running", false);
            anim.SetBool("Attacking", false);
        }
    }
    private void AttackPlayer()
    {
        zombieAgent.SetDestination(transform.position);
        transform.LookAt(lookPoint);
        if (!previouslyAttacked)
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(AttackingRaycastArea.transform.position, AttackingRaycastArea.transform.forward, out hitInfo, attackingRadius))
            {
                Debug.Log("Attacking " + hitInfo.transform.name);
                Player playerBody = hitInfo.transform.GetComponent<Player>();

                if (playerBody != null)
                {
                    playerBody.PlayerHitDamage(giveDamage);
                }
                //animations
                anim.SetBool("Attacking", true);
                anim.SetBool("Running", false);
            }

            previouslyAttacked = true;
            Invoke(nameof(ActiveAttacking), timeBtwAttack);
        }
    }

    private void ActiveAttacking()
    {
        previouslyAttacked = false;
    }

    public void zombieHitDamage(float takeDamage)
    {
        presentHealth -= takeDamage;

        healthBar.SetHealth(presentHealth);
        if (presentHealth <= 0)
        {
            //animations
            anim.SetBool("Died", true);
            anim.SetBool("Attacking", false);
            zombieDie();
        }
    }

    void zombieDie()
    {
        // audio
        audioSource.PlayOneShot(death);

        zombieAgent.SetDestination(transform.position);
        zombieSpeed = 0;
        attackingRadius = 0;
        visionRadius = 0;
        playerInAttackingRadius = false;
        playerInVisionRadius = false;
        Object.Destroy(gameObject, 5f);

    }
}
