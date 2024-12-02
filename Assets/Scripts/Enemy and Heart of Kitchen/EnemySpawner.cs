using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // The enemy prefab to spawn
    public Transform spawnPoint; // The spawn point where enemies will appear
    public float spawnInterval = 2f; // Time interval between spawns (in seconds)

    private Coroutine spawnCoroutine; // Reference to the active spawning coroutine
    private bool isActive = false; // Tracks whether the spawner is active

    private void Start()
    {
        // Ensure spawn point is assigned
        if (spawnPoint == null)
        {
            Debug.LogError("Spawn point is not assigned! Please assign a spawn point.");
            return;
        }

        // Start the spawner by default when initialized
        SetSpawnerActive(true);
    }

    // Activate or deactivate the spawner
    public void SetSpawnerActive(bool active)
    {
        if (active && !isActive)
        {
            // If spawner is not active and should be activated
            isActive = true;
            spawnCoroutine = StartCoroutine(SpawnEnemies());
        }
        else if (!active && isActive)
        {
            // If spawner is active and should be deactivated
            isActive = false;
            if (spawnCoroutine != null)
            {
                StopCoroutine(spawnCoroutine);
                spawnCoroutine = null;
            }
        }
    }

    private IEnumerator SpawnEnemies()
    {
        while (isActive)
        {
            Debug.Log("Spawning Enemy");
            // Spawn an enemy at the spawn point's position and rotation
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            yield return new WaitForSeconds(spawnInterval); // Wait for the next spawn
        }
    }

    private void OnDisable()
    {
        // Ensure coroutine is stopped if the GameObject is disabled
        // Ensure coroutine is stopped if the GameObject is disabled
        SetSpawnerActive(false);
    }
}

