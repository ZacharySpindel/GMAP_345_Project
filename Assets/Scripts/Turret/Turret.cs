using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float forwardForce = 10f;
    public float initialFireDelay = 1f; // Public initial delay variable

    private Transform target;
    private float fireCooldown;
    private float fireDelayTimer;

    private bool hasPlayedSound = false; 
    // Tracks if the sound has been played for the current target

    private void Start()
    {
        fireCooldown = 0f;
        fireDelayTimer = initialFireDelay; // Set the delay timer to the initial delay
    }

    private void Update()
    {
        // Wait until the initial delay timer has counted down to zero
        if (fireDelayTimer > 0f)
        {
            fireDelayTimer -= Time.deltaTime;
            return;
        }

        // Proceed with targeting and firing after the initial delay
        if (target != null)
        {
            RotateTowardsTarget();

            if (fireCooldown <= 0f)
            {
                Shoot();
                fireCooldown = 1f / fireRate;
            }
            fireCooldown -= Time.deltaTime;
        }
    }

    private void RotateTowardsTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    private void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            hasPlayedSound = false;
            rb.AddForce(shootPoint.forward * forwardForce, ForceMode.Impulse);
        }

        // Play sound effect only once when acquiring a new target
        if (!hasPlayedSound)
        {
            AudioManager.Instance.PlaySFX("Magic_Fire Ball");
            hasPlayedSound = true; // Mark sound as played
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && target == null)
        {
            target = other.transform;

            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") && other.transform == target)
        {
            target = null;
        }
    }
}
