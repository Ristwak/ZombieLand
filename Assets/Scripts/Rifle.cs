using System.Collections;
using UnityEngine;

public class Rifle : MonoBehaviour
{
    [Header("Rifle Things")]
    public Camera cam;
    public float giveDamageOf = 10f;
    public float shootingRange = 100f;
    public float fireCharge = 15f;
    public float nextTimeToShoot = 0f;
    public Animator animator;
    public Player player;
    public Transform hand;
    public GameObject RifleUI;

    [Header("Rifle Ammunition and Shooting")]
    public int maxAmmunition = 32;
    public int mag = 5;
    public int presentAmmunition;
    public float reloadingTime = 3f;
    public bool setReloading = false;
    public GameObject goreEffect;

    [Header("Rifle Effect")]
    public ParticleSystem muzzleSpark;
    public GameObject woodenEffect;

    [Header("Sound and UI")]
    public GameObject ammoOut;
    public AudioSource audioSource;
    public AudioClip shootingSound;
    public AudioClip reloadingSound;

    private float originalPlayerSpeed;
    private float originalPlayerSprint;
    private bool hasSavedPlayerSpeed;
    private Coroutine ammoOutRoutine;

    private void Awake()
    {
        if (hand != null) transform.SetParent(hand);
        presentAmmunition = Mathf.Max(0, maxAmmunition);
        if (RifleUI != null) RifleUI.SetActive(true);

        if (player != null)
        {
            originalPlayerSpeed = player.playerSpeed;
            originalPlayerSprint = player.playerSprint;
            hasSavedPlayerSpeed = true;
        }
    }

    private void Update()
    {
        if (setReloading) return;

        if (presentAmmunition <= 0)
        {
            if (mag > 0) StartCoroutine(Reload());
            else ShowAmmoOut();
            return;
        }

        bool firing = Input.GetButton("Fire1");
        bool aiming = Input.GetButton("Fire2");
        bool moving = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);

        if (firing && Time.time >= nextTimeToShoot)
        {
            if (animator != null)
            {
                animator.SetBool("Idle", false);
                animator.SetBool("Fire", true);
            }

            nextTimeToShoot = Time.time + 1f / Mathf.Max(0.01f, fireCharge);
            Shoot();
        }
        else if (firing && aiming && moving)
        {
            if (animator != null)
            {
                animator.SetBool("Idle", false);
                animator.SetBool("FireWalk", true);
                animator.SetBool("IdleAim", true);
                animator.SetBool("Walk", true);
                animator.SetBool("Reloading", false);
            }
        }
        else if (firing && moving)
        {
            if (animator != null)
            {
                animator.SetBool("Idle", false);
                animator.SetBool("FireWalk", true);
            }
        }
        else if (animator != null)
        {
            animator.SetBool("Idle", true);
            animator.SetBool("FireWalk", false);
            animator.SetBool("Fire", false);
        }
    }

    private void Shoot()
    {
        if (presentAmmunition <= 0) return;

        presentAmmunition--;

        if (presentAmmunition == 0 && mag > 0) mag--;

        UpdateUI();

        if (muzzleSpark != null) muzzleSpark.Play();
        if (audioSource != null && shootingSound != null) audioSource.PlayOneShot(shootingSound);

        if (cam == null) return;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hitInfo, shootingRange))
        {
            ObjectToHit objectToHit = hitInfo.transform.GetComponentInParent<ObjectToHit>();
            Zombie1 zombie1 = hitInfo.transform.GetComponentInParent<Zombie1>();
            StandingZombie standingZombie = hitInfo.transform.GetComponentInParent<StandingZombie>();

            if (objectToHit != null)
            {
                objectToHit.ObjectHitDamage(giveDamageOf);
                SpawnEffect(woodenEffect, hitInfo);
            }
            else if (zombie1 != null)
            {
                zombie1.zombieHitDamage(giveDamageOf);
                SpawnEffect(goreEffect, hitInfo);
            }
            else if (standingZombie != null)
            {
                standingZombie.zombieHitDamage(giveDamageOf);
                SpawnEffect(goreEffect, hitInfo);
            }
        }
    }

    private void SpawnEffect(GameObject effect, RaycastHit hitInfo)
    {
        if (effect == null) return;
        GameObject spawned = Instantiate(effect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
        Destroy(spawned, 1f);
    }

    private IEnumerator Reload()
    {
        if (setReloading || mag <= 0 || presentAmmunition > 0) yield break;

        setReloading = true;

        if (player != null)
        {
            player.playerSprint = 0f;
        }

        if (audioSource != null && reloadingSound != null) audioSource.PlayOneShot(reloadingSound);
        if (animator != null) animator.SetBool("Reloading", true);

        yield return new WaitForSeconds(reloadingTime);

        presentAmmunition = Mathf.Max(0, maxAmmunition);

        if (animator != null) animator.SetBool("Reloading", false);

        if (player != null && hasSavedPlayerSpeed)
        {
            player.playerSpeed = originalPlayerSpeed;
            player.playerSprint = originalPlayerSprint;
        }

        setReloading = false;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (AmmoCount.occurence == null) return;

        AmmoCount.occurence.AmmoText(presentAmmunition);
        AmmoCount.occurence.MagText(mag);
    }

    private void ShowAmmoOut()
    {
        if (ammoOut == null || ammoOutRoutine != null) return;
        ammoOutRoutine = StartCoroutine(showAmmoOut());
    }

    IEnumerator showAmmoOut()
    {
        ammoOut.SetActive(true);
        yield return new WaitForSeconds(5f);

        if (ammoOut != null) ammoOut.SetActive(false);
        ammoOutRoutine = null;
    }

    private void OnDisable()
    {
        if (player != null && hasSavedPlayerSpeed)
        {
            player.playerSpeed = originalPlayerSpeed;
            player.playerSprint = originalPlayerSprint;
        }

        setReloading = false;
    }
}