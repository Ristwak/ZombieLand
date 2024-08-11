using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class VehicleController : MonoBehaviour
{
    [Header("Wheel Collider")]
    public WheelCollider FrontRightWheelCollider;
    public WheelCollider FrontLeftWheelCollider;
    public WheelCollider BackRightWheelCollider;
    public WheelCollider BackLeftWheelCollider;

    [Header("Wheel Transformer")]
    public Transform FrontRightWheelTransformer;
    public Transform FrontLeftWheelTransformer;
    public Transform BackRightWheelTransformer;
    public Transform BackLeftWheelTransformer;
    public Transform vehicleDoor;

    [Header("Vehicle Engine")]
    public float accelerationForce = 100f;
    public float brakeForce = 200f;
    private float presentBrakeForce = 0f;
    private float presentAcceleration = 0f;

    [Header("Vehicle Steering")]
    public float wheelsTorque = 50f;
    private float presentTurnAngle = 0f;

    [Header("Vehicle Security")]
    public Player player;
    private float radius = 5f;
    private bool isOpened = false;

    [Header("Disable Things")]
    public GameObject AimCam;
    public GameObject AimCanvas;
    public GameObject ThirdPersonCam;
    public GameObject ThirdPersonCanvas;
    public GameObject PlayerCharacter;
    public ObjectivesComplete objectivesComplete;

    [Header("Vehicle Hit Var")]
    public Camera cam;
    public float hitRange = 2f;
    private float giveDamageOf = 100f;
    public GameObject goreEffect;

    [Header("Rifle Effect")]
    public GameObject woodenEffect;

    private void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < radius)
        {
            if (Input.GetKeyDown("f"))
            {
                isOpened = true;
                radius = 5000f;

                //objective completed
                objectivesComplete.GetObjectivesDone3(true);
            }
            else if (isOpened == true && Input.GetKeyDown("g"))
            {
                player.transform.position = vehicleDoor.transform.position;
                isOpened = false;
                radius = 5f;
            }
        }

        if (isOpened == true)
        {
            ThirdPersonCam.SetActive(false);
            ThirdPersonCanvas.SetActive(false);
            AimCam.SetActive(false);
            AimCanvas.SetActive(false);
            PlayerCharacter.SetActive(false);

            MoveVehicle();
            VehicleSteering();
            ApplyBrakes();
            HitZombies();
        }
        else if(isOpened == false)
        {
            ThirdPersonCam.SetActive(true);
            ThirdPersonCanvas.SetActive(true);
            AimCam.SetActive(true);
            AimCanvas.SetActive(true);
            PlayerCharacter.SetActive(true);
        }

    }
    void MoveVehicle()
    {
        FrontRightWheelCollider.motorTorque = presentAcceleration;
        FrontLeftWheelCollider.motorTorque = presentAcceleration;
        BackRightWheelCollider.motorTorque = presentAcceleration;
        BackLeftWheelCollider.motorTorque = presentAcceleration;
        presentAcceleration = accelerationForce * -Input.GetAxis("Vertical");
    }

    void VehicleSteering()
    {
        presentTurnAngle = wheelsTorque * Input.GetAxis("Horizontal");
        FrontRightWheelCollider.steerAngle = presentTurnAngle;
        FrontLeftWheelCollider.steerAngle = presentTurnAngle;

        // animate the wheels
        SteeringWheels(FrontRightWheelCollider, FrontRightWheelTransformer);
        SteeringWheels(FrontLeftWheelCollider, FrontLeftWheelTransformer);
        SteeringWheels(BackRightWheelCollider, BackRightWheelTransformer);
        SteeringWheels(BackLeftWheelCollider, BackLeftWheelTransformer);
    }

    void SteeringWheels(WheelCollider WC, Transform WT)
    {
        Vector3 position;
        Quaternion rotation;

        WC.GetWorldPose(out position, out rotation);

        WT.position = position;
        WT.rotation = rotation;
    }

    void ApplyBrakes()
    {
        if (Input.GetKey(KeyCode.Space))
            presentBrakeForce = brakeForce;
        else
            presentBrakeForce = 0f;

        FrontRightWheelCollider.brakeTorque = presentBrakeForce;
        FrontLeftWheelCollider.brakeTorque = presentBrakeForce;
        BackRightWheelCollider.brakeTorque = presentBrakeForce;
        BackLeftWheelCollider.brakeTorque = presentBrakeForce;
    }

    void HitZombies()
    {
        RaycastHit hitInfo;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, hitRange))
        {
            Zombie1 zombie1 = hitInfo.transform.GetComponent<Zombie1>();
            ObjectToHit objectToHit = hitInfo.transform.GetComponent<ObjectToHit>();
            StandingZombie Standingzombie = hitInfo.transform.GetComponent<StandingZombie>();

            if (objectToHit != null)
            {
                objectToHit.ObjectHitDamage(giveDamageOf);
                GameObject WoodGo = Instantiate(woodenEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(WoodGo, 1f);
            }
            else if(zombie1 != null)
            {
                zombie1.zombieHitDamage(giveDamageOf);
                zombie1.GetComponent<CapsuleCollider>().enabled = false;
                GameObject goreEffectGo = Instantiate(goreEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(goreEffectGo, 1f);
            }
            else if(Standingzombie != null)
            {
                Standingzombie.zombieHitDamage(giveDamageOf);
                Standingzombie  .GetComponent<CapsuleCollider>().enabled = false;
                GameObject goreEffectGo = Instantiate(goreEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(goreEffectGo, 1f);
            }
        }
    }
}
