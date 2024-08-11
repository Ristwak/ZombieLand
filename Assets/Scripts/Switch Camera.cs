using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    [Header("Camera to Assign")]
    public GameObject AimCam;
    public GameObject AimCanvas;
    public GameObject TPSCam;
    public GameObject TPSCanvas;

    [Header("Camera Animator")]
    public Animator animator;

    

    private void Update()
    {
        if (Input.GetButton("Fire2") && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {

            animator.SetBool("Idle", false);
            animator.SetBool("IdleAim", true);
            animator.SetBool("RifleWalk", true);
            animator.SetBool("Walk", true);


            TPSCam.SetActive(false);
            TPSCanvas.SetActive(false);
            AimCam.SetActive(true);
            AimCanvas.SetActive(true);
        }
        else if(Input.GetButton("Fire2"))
        {
            animator.SetBool("Idle", false);
            animator.SetBool("IdleAim", true);
            animator.SetBool("RifleWalk", false);
            animator.SetBool("Walk", false);


            TPSCam.SetActive(false);
            TPSCanvas.SetActive(false);
            AimCam.SetActive(true);
            AimCanvas.SetActive(true);
        }
        else
        {
            animator.SetBool("Idle", true);
            animator.SetBool("IdleAim", false);
            animator.SetBool("RifleWalk", false);


            TPSCam.SetActive(true);
            TPSCanvas.SetActive(true);
            AimCam.SetActive(false);
            AimCanvas.SetActive(false);
        }
    }
}
