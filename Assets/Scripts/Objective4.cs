using UnityEngine;
using UnityEngine.SceneManagement;

public class Objective4 : MonoBehaviour
{
    public ObjectivesComplete objectivesComplete;
    private bool completed;

    private void OnTriggerEnter(Collider other)
    {
        if (completed || !other.CompareTag("Vehicle")) return;

        completed = true;

        if (objectivesComplete != null)
            objectivesComplete.GetObjectivesDone4(true);

        SceneManager.LoadScene("MainMenu");
    }
}