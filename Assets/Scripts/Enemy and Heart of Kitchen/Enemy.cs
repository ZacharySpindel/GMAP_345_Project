using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int health = 10;               // Initial health for the enemy
    public float moveSpeed = 3.5f;        // Enemy move speed
    public int enemyDamage = 5;           // Damage dealt to the goal upon reaching it
    public GameObject goldPrefab;         // Gold prefab to drop on death

    private NavMeshAgent agent;
    private Transform goal;

    void Start()
    {
        // Setup NavMeshAgent for pathfinding
        agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.speed = moveSpeed;
        }

        // Find the goal object in the scene
        GameObject goalObject = GameObject.FindGameObjectWithTag("Goal");
        if (goalObject != null)
        {
            goal = goalObject.transform;
            agent.SetDestination(goal.position);
        }
        else
        {
            Debug.LogError("Goal object not found in the scene!");
        }
    }

    void Update()
    {
        // Destroy the enemy if health reaches zero or below
        if (health <= 0)
        {
            Die();
        }
    }

    // Detect triggers with the projectile
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            // Take damage and destroy the projectile
            TakeDamage(100); // Assuming each projectile does 1 damage
            Destroy(other.gameObject);
        }

        // Detect triggers with the goal
        if (other.CompareTag("Goal"))
        {
            Goal goalScript = other.GetComponent<Goal>();
            if (goalScript != null)
            {
                goalScript.TakeDamage(enemyDamage); // Damage the goal
            }

            // Destroy the enemy upon reaching the goal
            Die();
        }
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
    }

    private void Die()
    {
        // Spawn gold prefab upon death
        if (goldPrefab != null)
        {
            Instantiate(goldPrefab, transform.position, Quaternion.identity);
        }

        // Destroy the enemy object
        Destroy(gameObject);
    }
}
