using UnityEngine;

public class AddAmmo : MonoBehaviour
{
    [Header("Add Ammo")]
    public Rifle rifle;
    public int ammoToGive = 5;
    public float radius = 2.5f;

    [Header("Ammo Audio")]
    public AudioSource audioSource;
    public AudioClip addAmmoSound;

    [Header("Health Animation")]
    public Animator animator;

    private bool collected;

    private void Update()
    {
        if (collected || rifle == null) return;

        if (Vector3.Distance(transform.position, rifle.transform.position) >= radius) return;
        if (!Input.GetKeyDown(KeyCode.F)) return;

        collected = true;

        if (animator != null) animator.SetBool("Open", true);

        // Preserve the original pickup behaviour: set reserve magazines.
        rifle.mag = Mathf.Max(0, ammoToGive);

        if (audioSource != null && addAmmoSound != null)
            audioSource.PlayOneShot(addAmmoSound);

        Destroy(gameObject, 1.5f);
    }
}