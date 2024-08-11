using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectivesComplete : MonoBehaviour
{
    [Header("Objectives to Compare")]
    public Text Objective1;
    public Text Objective2;
    public Text Objective3;
    public Text Objective4;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip objecttiveCompleteSound;

    public static ObjectivesComplete occurence;

    private void Awake()
    {
        occurence = this;
    }

    public void GetObjectivesDone1(bool obj1)
    {
        if(obj1 == true)
        {
            Objective1.text = "1. Completed";
            Objective1.color = Color.green;
            audioSource.PlayOneShot(objecttiveCompleteSound);
        }
        else
        {
            Objective1.text = "1. Find The Rifle";
            Objective1.color = Color.white;
        }
    }
    public void GetObjectivesDone2(bool obj2)
    {
        
        if(obj2 == true)
        {
            Objective2.text = "2. Completed";
            Objective2.color = Color.green;
            audioSource.PlayOneShot(objecttiveCompleteSound);
        }
        else
        {
            Objective2.text = "2. Find Survivours";
            Objective2.color = Color.white;
        }
    }
    public void GetObjectivesDone3(bool obj3)
    {
        
        if(obj3 == true)
        {
            Objective3.text = "3. Completed";
            Objective3.color = Color.green;
            audioSource.PlayOneShot(objecttiveCompleteSound);
        }
        else
        {
            Objective3.text = "3. Find Vehicle";
            Objective3.color = Color.white;
        }
    }
    public void GetObjectivesDone4(bool obj4)
    {
        
        if(obj4 == true)
        {
            Objective4.text = "4. Completed";
            Objective4.color = Color.green;
            audioSource.PlayOneShot(objecttiveCompleteSound);
        }
        else
        {
            Objective4.text = "4. Save the Survivours";
            Objective4.color = Color.white;
        }
    }
}
