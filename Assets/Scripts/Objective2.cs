using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective2 : MonoBehaviour
{
    public ObjectivesComplete objectivesComplete;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            objectivesComplete.GetObjectivesDone2(true);

            Destroy(gameObject, 2f); // Corrected 'gameobject' to 'gameObject'
        }
    }
}
