using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrow : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab; // Assign the projectile prefab in the Inspector
    [SerializeField] private float throwForce = 10f; // Force with which the projectile is thrown
    [SerializeField] private Transform throwPoint; // Reference to the empty GameObject (throw point)
    private Player player; // Reference to the Player component

    private void Start()
    {
        player = GetComponent<Player>(); // Get the Player component
        if (player == null)
        {
            Debug.LogError("Player component not found!");
        }

        // Ensure the throwPoint is assigned
        if (throwPoint == null)
        {
            Debug.LogError("Throw point is not assigned!");
        }
    }

    private void Update()
    {
        // Check for input to throw the kitchen object
        if (Input.GetKeyDown(KeyCode.O))
        {
            ThrowKitchenObject();
        }
    }

    private void ThrowKitchenObject()
    {
        // Check if the player has a kitchen object
        if (player.HasKitchenObject())
        {
            // Get the kitchen object and its visual representation
            KitchenObject kitchenObject = player.GetKitchenObject();
            GameObject kitchenObjectVisual = kitchenObject.gameObject; // Assuming the kitchen object itself is the visual

            // Remove the kitchen object from the player
            player.ClearKitchenObject();

            // Hide the visual representation (optional)
            kitchenObjectVisual.SetActive(false);

            // Spawn the projectile at the throw point's position and rotation
            GameObject projectile = Instantiate(projectilePrefab, throwPoint.position, throwPoint.rotation);

            // Set the collider of the projectile to trigger mode (if not already done in the prefab)
            Collider projectileCollider = projectile.GetComponent<Collider>();
            if (projectileCollider != null)
            {
                projectileCollider.isTrigger = true;
            }

            // Get the Rigidbody component and apply the throw force
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Apply force to the projectile in the direction the throw point is facing
                rb.AddForce(throwPoint.forward * throwForce, ForceMode.Impulse);

                // Disable bounce and other reactions after the throw
                rb.drag = 0; // Disable linear drag if not needed (optional)
                rb.angularDrag = 0; // Disable angular drag if not needed (optional)
                rb.useGravity = false; // Disable gravity if you don't want it to fall (optional)

                // Optional: Set the Rigidbody's velocity to simulate smooth movement
                rb.velocity = rb.velocity.normalized * throwForce; // Ensure constant speed, adjust for your needs
            }
            else
            {
                Debug.LogError("Projectile prefab does not have a Rigidbody component!");
            }
        }
    }
}
