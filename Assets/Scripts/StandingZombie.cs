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
    private bool isDead;

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
        if (zombieAgent == null) zombieAgent = GetComponent<NavMeshAgent>();
        if (healthBar != null) healthBar.GiveFullHealth(zombieHealth);
    }

    private void Update()
    {
        if (isDead || zombieAgent == null || !zombieAgent.enabled) return;

        playerInVisionRadius = Physics.CheckSphere(transform.position, visionRadius, PlayerLayer);
        playerInAttackingRadius = Physics.CheckSphere(transform.position, attackingRadius, PlayerLayer);

        if (!playerInVisionRadius && !playerInAttackingRadius) Idle();
        else if (playerInVisionRadius && !playerInAttackingRadius) PursuePlayer();
        else if (playerInVisionRadius && playerInAttackingRadius) AttackPlayer();
    }

    private void Idle()
    {
        zombieAgent.SetDestination(transform.position);

        if (anim != null)
        {
            anim.SetBool("Idle", true);
            anim.SetBool("Running", false);
            anim.SetBool("Attacking", false);
        }
    }

    private void PursuePlayer()
    {
        if (PlayerBody == null) return;

        zombieAgent.speed = zombieSpeed;
        zombieAgent.SetDestination(PlayerBody.position);

        if (anim != null)
        {
            anim.SetBool("Idle", false);
            anim.SetBool("Running", true);
            anim.SetBool("Attacking", false);
        }
    }

    private void AttackPlayer()
    {
        zombieAgent.SetDestination(transform.position);

        if (lookPoint != null) transform.LookAt(lookPoint);

        if (previouslyAttacked) return;

        if (AttackingRaycastArea != null &&
            Physics.Raycast(AttackingRaycastArea.transform.position,
            AttackingRaycastArea.transform.forward, out RaycastHit hitInfo, attackingRadius))
        {
            Player target = hitInfo.transform.GetComponentInParent<Player>();

            if (target != null)
            {
                target.PlayerHitDamage(giveDamage);

                if (anim != null)
                {
                    anim.SetBool("Attacking", true);
                    anim.SetBool("Running", false);
                }
            }
        }

        previouslyAttacked = true;
        Invoke(nameof(ActiveAttacking), Mathf.Max(0.05f, timeBtwAttack));
    }

    private void ActiveAttacking()
    {
        previouslyAttacked = false;
    }

    public void zombieHitDamage(float takeDamage)
    {
        if (isDead || takeDamage <= 0f) return;

        presentHealth = Mathf.Max(0f, presentHealth - takeDamage);
        if (healthBar != null) healthBar.SetHealth(presentHealth);

        if (presentHealth <= 0f) zombieDie();
    }

    void zombieDie()
    {
        if (isDead) return;
        isDead = true;

        if (anim != null)
        {
            anim.SetBool("Died", true);
            anim.SetBool("Attacking", false);
            anim.SetBool("Running", false);
        }

        if (audioSource != null && death != null) audioSource.PlayOneShot(death);

        if (zombieAgent != null && zombieAgent.enabled)
        {
            zombieAgent.isStopped = true;
            zombieAgent.ResetPath();
        }

        CancelInvoke();
        enabled = false;
        Destroy(gameObject, 5f);
    }
}