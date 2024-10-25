using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int health = 10;               // Initial health for the enemy
    public float moveSpeed = 3.5f;        // Enemy move speed
    public int enemyDamage = 5;           // Damage dealt to the goal upon reaching it

    private NavMeshAgent agent;
    private Transform goal;

    void Start()
    {
        // Setup NavMeshAgent for pathfinding
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;

        // Find the goal object in the scene
        goal = GameObject.FindGameObjectWithTag("Goal").transform;
        agent.SetDestination(goal.position);
    }

    void Update()
    {
        // Check if health is 0 or below, then destroy enemy
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    // Detect collisions with the projectile
    private void OnCollisionEnter(Collision collision)
    {
        // Check if hit by a projectile
        if (collision.gameObject.CompareTag("Projectile"))
        {
            TakeDamage(health);           // Sets health to 0, destroying the enemy
            Destroy(collision.gameObject); // Destroy the projectile upon impact
        }
    }

    // Detect triggers with the goal
    private void OnTriggerEnter(Collider other)
    {
        // Check if collided with the goal
        if (other.CompareTag("Goal"))
        {
            Goal goalScript = other.GetComponent<Goal>();
            if (goalScript != null)
            {
                // Damage the goal using the enemyDamage variable, then set health to 0
                goalScript.TakeDamage(enemyDamage);
                health = 0;
            }
        }
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
    }
}
