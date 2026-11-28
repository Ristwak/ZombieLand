using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player")]
    public float playerSpeed = 1.9f;
    public float playerSprint = 3f;

    [Header("Player Health things")]
    private float playerHealth = 100f;
    public float presentHealth;
    public GameObject playerDamage;
    public HealthBar healthBar;

    [Header("Player Script Camera")]
    public Transform playerCamera;
    public GameObject EndGameMenuUi;

    [Header("Player Animator and Gravity")]
    public CharacterController cc;
    public float gravity = -9.81f;
    public Animator animator;

    [Header("Player Jumping and Velocity")]
    public float turnCalmTime = 0.1f;
    float turnCalmVelocity;
    public float jumpRange = 1f;
    Vector3 velocity;
    public Transform surfaceCheck;
    bool onSurface;
    public float surfaceDistance = 0.4f;
    public LayerMask surfaceMask;

    [Header("Player Sound")]
    public AudioSource audioSource;
    public AudioClip hurt;

    private bool isDead;
    private Coroutine damageRoutine;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        presentHealth = playerHealth;
        if (healthBar != null) healthBar.GiveFullHealth(playerHealth);
    }

    private void Update()
    {
        if (isDead || cc == null) return;

        if (surfaceCheck != null)
            onSurface = Physics.CheckSphere(surfaceCheck.position, surfaceDistance, surfaceMask);

        if (onSurface && velocity.y < 0) velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);

        PlayerMove();
        Jump();
    }

    void PlayerMove()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        bool sprinting = Input.GetButton("Sprint") && Input.GetKey(KeyCode.W) && onSurface;
        float speed = sprinting ? playerSprint : playerSpeed;

        if (direction.magnitude >= 0.1f)
        {
            if (animator != null)
            {
                animator.SetBool("Idle", false);
                animator.SetBool("Walk", !sprinting);
                animator.SetBool("Running", sprinting);
                animator.SetBool("RifleWalk", false);
                animator.SetBool("IdleAim", false);
            }

            if (playerCamera != null)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
                Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 10f);
                Vector3 moveDirection = rotation * Vector3.forward;
                cc.Move(moveDirection.normalized * speed * Time.deltaTime);
            }
        }
        else if (animator != null)
        {
            animator.SetBool("Idle", true);
            animator.SetBool("Walk", false);
            animator.SetBool("Running", false);
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && onSurface)
        {
            if (animator != null)
            {
                animator.SetBool("Idle", false);
                animator.SetTrigger("Jump");
            }

            velocity.y = Mathf.Sqrt(jumpRange * -2f * gravity);
        }
        else if (animator != null)
        {
            animator.ResetTrigger("Jump");
        }
    }

    public void PlayerHitDamage(float takeDamage)
    {
        if (isDead || takeDamage <= 0f) return;

        presentHealth = Mathf.Max(0f, presentHealth - takeDamage);

        if (damageRoutine != null) StopCoroutine(damageRoutine);
        if (playerDamage != null) damageRoutine = StartCoroutine(PlayerDamage());

        if (audioSource != null && hurt != null) audioSource.PlayOneShot(hurt);
        if (healthBar != null) healthBar.SetHealth(presentHealth);

        if (presentHealth <= 0f) PlayerDie();
    }

    void PlayerDie()
    {
        if (isDead) return;
        isDead = true;

        if (EndGameMenuUi != null) EndGameMenuUi.SetActive(true);
        Cursor.lockState = CursorLockMode.None;

        if (cc != null) cc.enabled = false;
        Destroy(gameObject, 1f);
    }

    IEnumerator PlayerDamage()
    {
        playerDamage.SetActive(true);
        yield return new WaitForSeconds(2.18f);

        if (playerDamage != null) playerDamage.SetActive(false);
        damageRoutine = null;
    }
}