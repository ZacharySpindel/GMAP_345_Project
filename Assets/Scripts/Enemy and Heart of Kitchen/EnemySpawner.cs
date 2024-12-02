using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // The enemy prefab to spawn
    public Transform spawnPoint; // The spawn point where enemies will appear
    public float spawnInterval = 2f; // Time interval between spawns (in seconds)

    private Coroutine spawnCoroutine; // Reference to the active spawning coroutine
    private bool isActive = false; // Tracks whether the spawner is active

    // Activate or deactivate the spawner
    public void SetSpawnerActive(bool active)
    {
        if (active && !isActive)
        {
            isActive = true;
            spawnCoroutine = StartCoroutine(SpawnEnemies());
        }
        else if (!active && isActive)
        {
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
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void OnDisable()
    {
        SetSpawnerActive(false); // Ensure the coroutine stops if the spawner is disabled
    }


    


}
