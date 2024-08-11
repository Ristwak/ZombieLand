using System.Collections;
using System.Collections.Generic;
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

    private void Update()
    {
        if(Vector3.Distance(transform.position, rifle.transform.position) < radius)
        {
            if(Input.GetKeyDown("f"))
            {
                animator.SetBool("Open",true);
                rifle.mag = ammoToGive;

                // sound effect
                audioSource.PlayOneShot(addAmmoSound);


                Object.Destroy(gameObject,1.5f);
            }
        }
    }
}
