using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patron : MonoBehaviour
{
    private enum patronType { Customer, Recruit, Hire }
    private patronType currentStatus; // Current state of patrons
    private float correctOrderCounter;  // how many correct orders you provide
    private float recruitThreshold = 30; // Points needed to change states

    public DayNightDuskCycle dayNightDuskCycle;

    public string patronTableID;

    // Start is called before the first frame update
    void Start()
    {
        currentStatus = patronType.Customer; // Start with Customer
        BackToTable();
        // find table that matches IDs
        // get that table's position
        // move to that table (NavMesh?)
    }

    // Update is called once per frame
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
    }

    // a patron needs to move to tables associated to them
    public void BackToTable()
    {

    }
    // a recruited patron needs to go back into customer when dawn hits (we need dawn as well)
    public void RecruitToCustomer()
    {
        if (dayNightDuskCycle.currentTimeOfDay == DayNightDuskCycle.TimeOfDay.Dawn && currentStatus == patronType.Recruit)
        {
            currentStatus = patronType.Customer;
        }
    }

    // patrons will attack enemies with projectiles (?)
    public void AttackEnemies()
    {
        // aim at enemies
        // shoot projectile
    }

    // math behind patron recruit points
    public void PatronRecruitPoints()
    {
        //lettuce & tomato = 5, cheese = 10, meat & bun = 5


    }
}