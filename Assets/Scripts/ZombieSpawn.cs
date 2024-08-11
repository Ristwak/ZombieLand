using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawn : MonoBehaviour
{
    [Header("Zombie Spawn Var")]
    public GameObject ZombiePrefab;
    public Transform ZombieSpawnPosition1;
    public Transform ZombieSpawnPosition2;
    public Transform ZombieSpawnPosition3;
    public GameObject DangerZone;
    private float repeatCycle = 1f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip zombieRoar;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Vehicle" )
        {
            InvokeRepeating("EnemySpawner", 1f, repeatCycle);
            StartCoroutine(dangerZoneTimer());
            Destroy(gameObject, 10f);
            gameObject.GetComponent<BoxCollider>().enabled = false;
            audioSource.PlayOneShot(zombieRoar);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
    }

    void EnemySpawner()
    {
        Instantiate(ZombiePrefab, ZombieSpawnPosition1.position, ZombieSpawnPosition1.rotation);
        Instantiate(ZombiePrefab, ZombieSpawnPosition2.position, ZombieSpawnPosition2.rotation);
        Instantiate(ZombiePrefab, ZombieSpawnPosition3.position, ZombieSpawnPosition3.rotation);
    }

    IEnumerator dangerZoneTimer()
    {
        DangerZone.SetActive(true);
        yield return new WaitForSeconds(5f);
        DangerZone.SetActive(false);
    }
}
