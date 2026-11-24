using System.Collections;
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

    private bool hasActivated;
    private Coroutine spawnRoutine;

    private void OnTriggerEnter(Collider other)
    {
        if (hasActivated || (!other.CompareTag("Player") && !other.CompareTag("Vehicle"))) return;

        hasActivated = true;

        if (DangerZone != null) StartCoroutine(dangerZoneTimer());
        if (audioSource != null && zombieRoar != null) audioSource.PlayOneShot(zombieRoar);

        spawnRoutine = StartCoroutine(EnemySpawner());
        Destroy(gameObject, 10f);

        BoxCollider trigger = GetComponent<BoxCollider>();
        if (trigger != null) trigger.enabled = false;
    }

    private IEnumerator EnemySpawner()
    {
        yield return new WaitForSeconds(1f);

        while (true)
        {
            SpawnZombie(ZombieSpawnPosition1);
            SpawnZombie(ZombieSpawnPosition2);
            SpawnZombie(ZombieSpawnPosition3);

            yield return new WaitForSeconds(Mathf.Max(0.1f, repeatCycle));
        }
    }

    private void SpawnZombie(Transform spawnPoint)
    {
        if (ZombiePrefab == null || spawnPoint == null) return;
        Instantiate(ZombiePrefab, spawnPoint.position, spawnPoint.rotation);
    }

    IEnumerator dangerZoneTimer()
    {
        DangerZone.SetActive(true);
        yield return new WaitForSeconds(5f);

        if (DangerZone != null) DangerZone.SetActive(false);
    }

    private void OnDestroy()
    {
        if (spawnRoutine != null) StopCoroutine(spawnRoutine);
    }
}