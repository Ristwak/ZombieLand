using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Objective4 : MonoBehaviour
{
    public ObjectivesComplete objectivesComplete;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Vehicle")
        {
            objectivesComplete.GetObjectivesDone4(true);
            
            SceneManager.LoadScene("MainMenu");
        }
    }
}
