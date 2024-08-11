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

    private void Awake()
    {
        // We used this so that the rifle sticks to the player hand
        transform.SetParent(hand);
        presentAmmunition = maxAmmunition;
        RifleUI.SetActive(true);
    }

    private void Update()
    {
        if (setReloading == true)
        {
            return;
        }

        if (presentAmmunition <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToShoot)
        {
            animator.SetBool("Idle", false);
            animator.SetBool("Fire", true);
            nextTimeToShoot = Time.time + 1 / fireCharge;
            Shoot();
        }
        else if (Input.GetButton("Fire1") && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            animator.SetBool("Idle", false);
            animator.SetBool("FireWalk", true);
        }
        else if (Input.GetButton("Fire1") && Input.GetButton("Fire2"))
        {
            animator.SetBool("Idle", false);
            animator.SetBool("FireWalk", true);
            animator.SetBool("IdleAim", true);
            animator.SetBool("Walk", true);
            animator.SetBool("Reloading", false);
        }
        else
        {
            animator.SetBool("Idle", true);
            animator.SetBool("FireWalk", false);
            animator.SetBool("Fire", false);
        }
    }

    private void Shoot()
    {
        // Check for mag
        if (mag == 0)
        {
            // Show ammo out text
            StartCoroutine(showAmmoOut());
            return;
        }

        presentAmmunition--;

        if (presentAmmunition == 0)
        {
            mag--;
        }

        // Updating the UI
        UpdateUI();

        muzzleSpark.Play();
        audioSource.PlayOneShot(shootingSound);

        RaycastHit hitInfo;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, shootingRange))
        {
            Debug.Log(hitInfo.transform.name);
            ObjectToHit objectToHit = hitInfo.transform.GetComponent<ObjectToHit>();
            Zombie1 zombie1 = hitInfo.transform.GetComponent<Zombie1>();
            StandingZombie Standingzombie = hitInfo.transform.GetComponent<StandingZombie>();

            if (objectToHit != null)
            {
                objectToHit.ObjectHitDamage(giveDamageOf);
                GameObject WoodGo = Instantiate(woodenEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(WoodGo, 1f);
            }
            else if (zombie1 != null)
            {
                zombie1.zombieHitDamage(giveDamageOf);
                GameObject goreEffectGo = Instantiate(goreEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(goreEffectGo, 1f);
            }
            else if (Standingzombie != null)
            {
                Standingzombie.zombieHitDamage(giveDamageOf);
                GameObject goreEffectGo = Instantiate(goreEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(goreEffectGo, 1f);
            }
        }
    }

    private IEnumerator Reload()
    {
        player.playerSprint = 0;
        setReloading = true;
        Debug.Log("Reloading.......");
        audioSource.PlayOneShot(reloadingSound);


        animator.SetBool("Reloading", true);

        yield return new WaitForSeconds(reloadingTime);
        // Play animation
        animator.SetBool("Reloading", false);

        presentAmmunition = maxAmmunition;
        player.playerSpeed = 1.9f;
        player.playerSprint = 3f;
        setReloading = false;

        // Update UI after reloading
        UpdateUI();
    }

    private void UpdateUI()
    {
        AmmoCount.occurence.AmmoText(presentAmmunition);
        AmmoCount.occurence.MagText(mag);
    }

    IEnumerator showAmmoOut()
    {
        ammoOut.SetActive(true);
        yield return new WaitForSeconds(5f);
        ammoOut.SetActive(false);
    }
}
