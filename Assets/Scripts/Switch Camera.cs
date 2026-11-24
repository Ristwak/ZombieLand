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
        bool aiming = Input.GetButton("Fire2");
        bool movingForward = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);

        if (aiming && movingForward)
        {
            if (animator != null)
            {
                animator.SetBool("Idle", false);
                animator.SetBool("IdleAim", true);
                animator.SetBool("RifleWalk", true);
                animator.SetBool("Walk", true);
            }

            SetCameraState(true);
        }
        else if (aiming)
        {
            if (animator != null)
            {
                animator.SetBool("Idle", false);
                animator.SetBool("IdleAim", true);
                animator.SetBool("RifleWalk", false);
                animator.SetBool("Walk", false);
            }

            SetCameraState(true);
        }
        else
        {
            if (animator != null)
            {
                animator.SetBool("Idle", true);
                animator.SetBool("IdleAim", false);
                animator.SetBool("RifleWalk", false);
                animator.SetBool("Walk", false);
            }

            SetCameraState(false);
        }
    }

    private void SetCameraState(bool aiming)
    {
        if (TPSCam != null) TPSCam.SetActive(!aiming);
        if (TPSCanvas != null) TPSCanvas.SetActive(!aiming);
        if (AimCam != null) AimCam.SetActive(aiming);
        if (AimCanvas != null) AimCanvas.SetActive(aiming);
    }
}