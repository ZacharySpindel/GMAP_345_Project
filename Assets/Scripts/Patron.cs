using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patron : MonoBehaviour
{
    private enum patronType { Customer, Recruit, Hire }
    private patronType currentStatus; // Current state of patrons
    private float correctOrderCounter;  // how many correct orders you provide
    private float recruitThreshold = 30; // Points needed to change states

    public Transform tablePosition;
    public Transform stationPosition;
    
    private UnityEngine.AI.NavMeshAgent navmesh;
    public float moveSpeed = 4f;    // patron move speed

// turret stuff
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float forwardForce = 10f;
    public float initialFireDelay = 1f; // Public initial delay variable

    private Transform target;
    private float fireCooldown;
    private float fireDelayTimer;
// end of turret stuff

    public DayNightDuskCycle dayNightDuskCycle;

    public string patronTableID;

    void Start()
    {
        currentStatus = patronType.Customer; // Start with Customer
        navmesh = GetComponent<UnityEngine.AI.NavMeshAgent>(); // sets up navmesh
        navmesh.speed = moveSpeed;
        MoveToPoint(tablePosition);
        // find table that matches IDs
        // get that table's position
        // move to that table (NavMesh?)
    }

    void Update()
    {
        // if the amount of correct orders exceeds this threshold, then change their status
        if (correctOrderCounter > recruitThreshold)
        {
            switch (currentStatus)
            {
                case patronType.Customer:
                    currentStatus = patronType.Recruit;
                    recruitThreshold = 90;  // for recruit to go into hire, its 90 points
                    Debug.Log("Customer to Recruit");
                    break;
                case patronType.Recruit:
                    currentStatus = patronType.Hire;
                    Debug.Log("Recruit to Permanent Hire");
                    break;
                case patronType.Hire:
                    break;
            }
        }

        // this is what the patrons will be doing as those states (dawn)

        if(dayNightDuskCycle.currentTimeOfDay == DayNightDuskCycle.TimeOfDay.Dawn){
            MoveToPoint(tablePosition);  // regardless as to what their status is, patrons always go back to their tables upon morning
        }

        // day
        else if(dayNightDuskCycle.currentTimeOfDay == DayNightDuskCycle.TimeOfDay.Day){
            // blank for now
        }
        // dusk
        else if(dayNightDuskCycle.currentTimeOfDay == DayNightDuskCycle.TimeOfDay.Dusk){
            MoveToPoint(stationPosition);   // moves to predetermined position
        }
        // night
        else if(dayNightDuskCycle.currentTimeOfDay == DayNightDuskCycle.TimeOfDay.Night){
            TurretMode();
        }
    }

    // a patron needs to move to tables associated to them
    public void MoveToPoint(Transform transform)
    {
        navmesh.SetDestination(transform.position);
    }
    
    // a recruited patron needs to go back into customer when dawn hits (we need dawn as well)
    public void RecruitToCustomer()
    {
        if (dayNightDuskCycle.currentTimeOfDay == DayNightDuskCycle.TimeOfDay.Dawn && currentStatus == patronType.Recruit)
        {
            currentStatus = patronType.Customer;
        }
    }

    // math behind patron recruit points
    public void PatronRecruitPoints()
    {
        //lettuce & tomato = 5, cheese = 10, meat & bun = 5


    }

    // turret mode

    private void TurretMode()
    {
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
            rb.AddForce(shootPoint.forward * forwardForce, ForceMode.Impulse);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){
            Debug.Log("collider works");
        }

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