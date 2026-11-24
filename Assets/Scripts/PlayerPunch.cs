using UnityEngine;

public class PlayerPunch : MonoBehaviour
{
    [Header("Player Punch Var")]
    public Camera cam;
    public float giveDamageOf = 10f;
    public float punchRange = 5f;

    public void Punch()
    {
        if (cam == null) return;

        if (!Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hitInfo, punchRange))
            return;

        ObjectToHit objectToHit = hitInfo.transform.GetComponentInParent<ObjectToHit>();
        Zombie1 zombie1 = hitInfo.transform.GetComponentInParent<Zombie1>();
        StandingZombie standingZombie = hitInfo.transform.GetComponentInParent<StandingZombie>();

        if (objectToHit != null)
            objectToHit.ObjectHitDamage(giveDamageOf);
        else if (zombie1 != null)
            zombie1.zombieHitDamage(giveDamageOf);
        else if (standingZombie != null)
            standingZombie.zombieHitDamage(giveDamageOf);
    }
}