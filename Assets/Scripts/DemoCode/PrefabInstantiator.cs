using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabInstantiator : MonoBehaviour
{
    // Public variable to assign the prefab in the Inspector
    public GameObject prefabToInstantiate;

    // Public variable to set the build point in the Inspector
    public Transform buildPoint;

    // Reference to the CashManager (you can assign it through the Inspector or get it dynamically)
    public CashManager cashManager;

    // Cost to spawn the turret
    public int turretCost = 25;

    void Update()
    {
        // Check if the "B" key is pressed
        if (Input.GetKeyDown(KeyCode.B))
        {
            // Check if the player has enough cash
            if (cashManager.GetCashValue() >= turretCost)
            {
                // Instantiate the prefab at the build point's position and rotation
                Instantiate(prefabToInstantiate, buildPoint.position, buildPoint.rotation);

                // Subtract the cost of the turret from the player's cash
                cashManager.SubtractToCash(turretCost);

                Debug.Log("Turret spawned! 25 cash deducted.");
            }
            else
            {
                // If the player doesn't have enough cash, show a message
                Debug.Log("Not enough cash to spawn turret!");
            }
        }
    }
}
