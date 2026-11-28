using System.Collections.Generic;
using UnityEngine;

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

    private readonly HashSet<int> hitObjects = new HashSet<int>();

    private void Update()
    {
        if (player == null) return;

        if (!isOpened && Vector3.Distance(transform.position, player.transform.position) < radius)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                isOpened = true;
                radius = 5000f;

                if (objectivesComplete != null)
                    objectivesComplete.GetObjectivesDone3(true);
            }
        }
        else if (isOpened && Input.GetKeyDown(KeyCode.G))
        {
            ExitVehicle();
        }

        SetPlayerAndCameraState();

        if (!isOpened) return;

        MoveVehicle();
        VehicleSteering();
        ApplyBrakes();
        HitZombies();
    }

    private void ExitVehicle()
    {
        if (vehicleDoor != null) player.transform.position = vehicleDoor.position;

        isOpened = false;
        radius = 5f;
        presentAcceleration = 0f;
        ApplyBrakes();
    }

    private void SetPlayerAndCameraState()
    {
        bool driving = isOpened;

        if (ThirdPersonCam != null) ThirdPersonCam.SetActive(!driving);
        if (ThirdPersonCanvas != null) ThirdPersonCanvas.SetActive(!driving);
        if (AimCam != null) AimCam.SetActive(!driving);
        if (AimCanvas != null) AimCanvas.SetActive(!driving);
        if (PlayerCharacter != null) PlayerCharacter.SetActive(!driving);
    }

    void MoveVehicle()
    {
        presentAcceleration = accelerationForce * -Input.GetAxis("Vertical");

        FrontRightWheelCollider.motorTorque = presentAcceleration;
        FrontLeftWheelCollider.motorTorque = presentAcceleration;
        BackRightWheelCollider.motorTorque = presentAcceleration;
        BackLeftWheelCollider.motorTorque = presentAcceleration;
    }

    void VehicleSteering()
    {
        presentTurnAngle = wheelsTorque * Input.GetAxis("Horizontal");

        FrontRightWheelCollider.steerAngle = presentTurnAngle;
        FrontLeftWheelCollider.steerAngle = presentTurnAngle;

        SteeringWheels(FrontRightWheelCollider, FrontRightWheelTransformer);
        SteeringWheels(FrontLeftWheelCollider, FrontLeftWheelTransformer);
        SteeringWheels(BackRightWheelCollider, BackRightWheelTransformer);
        SteeringWheels(BackLeftWheelCollider, BackLeftWheelTransformer);
    }

    void SteeringWheels(WheelCollider WC, Transform WT)
    {
        if (WC == null || WT == null) return;

        WC.GetWorldPose(out Vector3 position, out Quaternion rotation);
        WT.position = position;
        WT.rotation = rotation;
    }

    void ApplyBrakes()
    {
        presentBrakeForce = Input.GetKey(KeyCode.Space) ? brakeForce : 0f;

        if (FrontRightWheelCollider != null) FrontRightWheelCollider.brakeTorque = presentBrakeForce;
        if (FrontLeftWheelCollider != null) FrontLeftWheelCollider.brakeTorque = presentBrakeForce;
        if (BackRightWheelCollider != null) BackRightWheelCollider.brakeTorque = presentBrakeForce;
        if (BackLeftWheelCollider != null) BackLeftWheelCollider.brakeTorque = presentBrakeForce;
    }

    void HitZombies()
    {
        if (cam == null) return;

        if (!Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hitInfo, hitRange))
        {
            hitObjects.Clear();
            return;
        }

        Zombie1 zombie1 = hitInfo.transform.GetComponentInParent<Zombie1>();
        StandingZombie standingZombie = hitInfo.transform.GetComponentInParent<StandingZombie>();
        ObjectToHit objectToHit = hitInfo.transform.GetComponentInParent<ObjectToHit>();

        Component target = zombie1 != null ? (Component)zombie1 :
                           standingZombie != null ? standingZombie :
                           objectToHit;

        if (target == null || !hitObjects.Add(target.gameObject.GetInstanceID())) return;

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

    private void SpawnEffect(GameObject effect, RaycastHit hitInfo)
    {
        if (effect == null) return;

        GameObject spawned = Instantiate(effect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
        Destroy(spawned, 1f);
    }
}