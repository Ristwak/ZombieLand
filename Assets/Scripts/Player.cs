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

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        presentHealth = playerHealth;
        healthBar.GiveFullHealth(playerHealth);
    }

    private void Update()
    {
        onSurface = Physics.CheckSphere(surfaceCheck.position, surfaceDistance, surfaceMask);

        if (onSurface && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);
        PlayerMove();
        Jump();
        Sprint();
    }

    void PlayerMove()
    {
        float horizontal_axis = Input.GetAxis("Horizontal");
        float vertical_axis = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal_axis, 0f, vertical_axis).normalized;

        if (direction.magnitude >= 0.1f)
        {
            animator.SetBool("Idle", false);
            animator.SetBool("Walk", true);
            animator.SetBool("Running", false);
            animator.SetBool("RifleWalk", false);
            animator.SetBool("IdleAim", false);

            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;

            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 10f);

            Vector3 moveDirection = rotation * Vector3.forward;
            cc.Move(moveDirection.normalized * playerSpeed * Time.deltaTime);
        }
        else
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
            animator.SetBool("Idle", false);
            animator.SetTrigger("Jump");
            velocity.y = Mathf.Sqrt(jumpRange * -2 * gravity);
        }
        else
        {
            animator.SetBool("Idle", true);
            animator.ResetTrigger("Jump");
        }
    }

    void Sprint()
    {
        if (Input.GetButton("Sprint") && Input.GetKey(KeyCode.W) && onSurface)
        {
            float horizontal_axis = Input.GetAxis("Horizontal");
            float vertical_axis = Input.GetAxis("Vertical");

            Vector3 direction = new Vector3(horizontal_axis, 0f, vertical_axis).normalized;

            if (direction.magnitude >= 0.1f)
            {
                animator.SetBool("Walk", false);
                animator.SetBool("Running", true);

                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;

                Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 10f);

                Vector3 moveDirection = rotation * Vector3.forward;
                cc.Move(moveDirection.normalized * playerSprint * Time.deltaTime);
            }
            else
            {
                animator.SetBool("Walk", true);
                animator.SetBool("Running", false);
            }
        }
    }

    public void PlayerHitDamage(float takeDamage)
    {
        presentHealth -= takeDamage;
        StartCoroutine(PlayerDamage());

        audioSource.PlayOneShot(hurt);
        healthBar.SetHealth(presentHealth);
        if (presentHealth <= 0)
        {
            PlayerDie();
        }
    }

    void PlayerDie()
    {
        EndGameMenuUi.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Object.Destroy(gameObject, 1.0f);
    }

    IEnumerator PlayerDamage()
    {
        playerDamage.SetActive(true);
        yield return new WaitForSeconds(2.18f);
        playerDamage.SetActive(false);
    }
}
