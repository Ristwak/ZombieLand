using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPunch : MonoBehaviour
{
    [Header("Player Punch Var")]
    public Camera cam;
    public float giveDamageOf = 10f;
    public float punchRange = 5f;


    public void Punch()
    {
        RaycastHit hitInfo;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, punchRange))
        {
            Debug.Log(hitInfo.transform.name);
            ObjectToHit objectToHit = hitInfo.transform.GetComponent<ObjectToHit>();
            Zombie1 zombie1 = hitInfo.transform.GetComponent<Zombie1>();
            StandingZombie Standingzombie = hitInfo.transform.GetComponent<StandingZombie>();

            if (objectToHit != null)
            {
                objectToHit.ObjectHitDamage(giveDamageOf);
            }
            else if(zombie1 != null)
            {
                zombie1.zombieHitDamage(giveDamageOf);
            }
            else if(Standingzombie != null)
            {
                Standingzombie.zombieHitDamage(giveDamageOf);
            }
        }
    }
}
