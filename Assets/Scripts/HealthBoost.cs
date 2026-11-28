using UnityEngine;

public class HealthBoost : MonoBehaviour
{
    [Header("Health Boost")]
    public Player player;
    public float healthToGive = 120f;
    public float radius = 2.5f;

    [Header("Health Audio")]
    public AudioSource audioSource;
    public AudioClip healthBoostSound;

    [Header("Health Animation")]
    public Animator animator;

    private bool collected;

    private void Update()
    {
        if (collected || player == null) return;

        if (Vector3.Distance(transform.position, player.transform.position) >= radius) return;
        if (!Input.GetKeyDown(KeyCode.F)) return;

        collected = true;

        if (animator != null) animator.SetBool("Open", true);

        // Preserve original 120 HP boost behaviour.
        player.presentHealth = Mathf.Max(player.presentHealth, healthToGive);

        if (player.healthBar != null)
            player.healthBar.SetHealth(player.presentHealth);

        if (audioSource != null && healthBoostSound != null)
            audioSource.PlayOneShot(healthBoostSound);

        Destroy(gameObject, 1.5f);
    }
}