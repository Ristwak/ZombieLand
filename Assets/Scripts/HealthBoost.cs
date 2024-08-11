using System.Collections;
using System.Collections.Generic;
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

    private void Update()
    {
        if(Vector3.Distance(transform.position, player.transform.position) < radius)
        {
            if(Input.GetKeyDown("f"))
            {
                animator.SetBool("Open",true);
                player.presentHealth = healthToGive;

                // sound effect
                audioSource.PlayOneShot(healthBoostSound);


                Object.Destroy(gameObject,1.5f);
            }
        }
    }
}