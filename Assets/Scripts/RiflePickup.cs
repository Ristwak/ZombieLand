using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiflePickup : MonoBehaviour
{
    [Header("Rifle's")]
    public GameObject PlayerRifle;
    public ObjectivesComplete objectivesComplete;
    public GameObject PickupRifle;
    public PlayerPunch playerPunch;
    public GameObject RifleUI;

    [Header("Rifle Assign Things")]
    public Player player;
    public float radius = 2.5f;
    public Animator animator;
    public float nextTimeToPunch = 0f;
    public float punchCharge = 15f;

    private void Awake()
    {
        PlayerRifle.SetActive(false);
        RifleUI.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetButtonDown("Fire1") && Time.time >= nextTimeToPunch)
        {
            animator.SetBool("Punch", true);
            animator.SetBool("Idle", false);
            nextTimeToPunch = Time.time + 1f/punchCharge;
            playerPunch.Punch();
        }
        else
        {
            animator.SetBool("Punch", false);
            animator.SetBool("Idle", true);
        }
        // To pickup the gun we added two gun one gun will be in the player's hand from the very start though it will be deactivated and another table will be lying somewhere and it'll have a randius in which if the player goes in and taps f button the gun which was in player's hand from the start will get activated again and the one player has to pick will be deactivaed which will seem like player has picked up the gun
        if (Vector3.Distance(transform.position, player.transform.position) < radius)
        {
            if (Input.GetKeyDown("f"))
            {
                PlayerRifle.SetActive(true);
                PickupRifle.SetActive(false);
                //pickup sound

                //objective completed
                objectivesComplete.GetObjectivesDone1(true);
            }
        }
    }
}
