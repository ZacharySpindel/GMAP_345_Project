using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerThrow : MonoBehaviour
{
    [SerializeField] private GameObject regularProjectilePrefab; // Regular projectile
    [SerializeField] private GameObject burgerProjectilePrefab;  // Burger AOE projectile
    [SerializeField] private GameObject saladProjectilePrefab;  // Salad projectile
    [SerializeField] private GameObject cheeseburgerProjectilePrefab;  // Cheeseburger projectile
    [SerializeField] private GameObject megaBurgerProjectilePrefab;  // MegaBurger projectile
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
            // Get the kitchen object
            KitchenObject kitchenObject = player.GetKitchenObject();
            GameObject kitchenObjectVisual = kitchenObject.gameObject; // The visual representation

            // Check if the kitchen object is a Plate
            if (kitchenObject is PlateKitchenObject plateKitchenObject)
            {
                // Check for specific ingredient combinations
                if (HasMegaBurgerIngredients(plateKitchenObject))
                {
                    Debug.Log("Plate contains MegaBurger ingredients. Spawning mega burger projectile.");
                    SpawnProjectile(megaBurgerProjectilePrefab);
                }
                else if (HasCheeseburgerIngredients(plateKitchenObject))
                {
                    Debug.Log("Plate contains Cheeseburger ingredients. Spawning cheeseburger projectile.");
                    SpawnProjectile(cheeseburgerProjectilePrefab);
                }
                else if (HasBurgerIngredients(plateKitchenObject))
                {
                    Debug.Log("Plate contains Burger ingredients. Spawning burger projectile.");
                    SpawnProjectile(burgerProjectilePrefab);
                }
                else if (HasSaladIngredients(plateKitchenObject))
                {
                    Debug.Log("Plate contains Salad ingredients. Spawning salad projectile.");
                    SpawnProjectile(saladProjectilePrefab);
                }
                else
                {
                    // Default to regular projectile if no recipe is matched
                    Debug.Log("Plate doesn't contain a known recipe. Spawning regular projectile.");
                    SpawnProjectile(regularProjectilePrefab);
                }
            }
            else
            {
                // If it's not a plate, spawn a regular projectile
                Debug.Log("Player is holding a non-plate item. Spawning regular projectile.");
                SpawnProjectile(regularProjectilePrefab);
            }

            // Hide the kitchen object visual representation (optional, if desired)
            kitchenObjectVisual.SetActive(false);

            // After throwing the kitchen object, remove it from the player (clear it)
            player.ClearKitchenObject();

            // Ensure the player is no longer holding the kitchen object
            if (!player.HasKitchenObject())
            {
                Debug.Log("Player no longer has a kitchen object after throw.");
            }
            else
            {
                Debug.Log("Player still has a kitchen object!");
            }
        }
        else
        {
            Debug.LogWarning("Player does not have a kitchen object to throw.");
        }
    }

    // Helper methods to check if the plate contains specific ingredients for each recipe

    private bool HasBurgerIngredients(PlateKitchenObject plateKitchenObject)
    {
        // Exactly 2 ingredients: Bread, MeatPattyCooked
        return ContainsExactlyIngredients(plateKitchenObject, new string[] { "Bread", "MeatPattyCooked" });
    }

    private bool HasCheeseburgerIngredients(PlateKitchenObject plateKitchenObject)
    {
        // Exactly 3 ingredients: Bread, MeatPattyCooked, CheeseSliced
        return ContainsExactlyIngredients(plateKitchenObject, new string[] { "Bread", "MeatPattyCooked", "CheeseSliced" });
    }

    private bool HasSaladIngredients(PlateKitchenObject plateKitchenObject)
    {
        // Exactly 2 ingredients: Tomato, Cabbage
        return ContainsExactlyIngredients(plateKitchenObject, new string[] { "Tomato", "Cabbage" });
    }

    private bool HasMegaBurgerIngredients(PlateKitchenObject plateKitchenObject)
    {
        // Exactly 5 ingredients: Bread, MeatPattyCooked, CheeseSliced, Tomato, Cabbage
        return ContainsExactlyIngredients(plateKitchenObject, new string[] { "Bread", "MeatPattyCooked", "CheeseSliced", "Tomato", "Cabbage" });
    }

    // Helper method to check if the plate contains exactly the required ingredients
    private bool ContainsExactlyIngredients(PlateKitchenObject plateKitchenObject, string[] requiredIngredients)
    {
        List<string> plateIngredients = new List<string>();

        // Add the names of the kitchen objects on the plate to the plateIngredients list
        foreach (KitchenObjectSO kitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
        {
            plateIngredients.Add(kitchenObjectSO.name);
        }

        // Sort both lists to ensure the order doesn't affect the comparison
        plateIngredients.Sort();
        List<string> requiredIngredientsList = new List<string>(requiredIngredients);
        requiredIngredientsList.Sort();

        // Compare the two lists: they must contain the exact same ingredients
        return plateIngredients.Count == requiredIngredientsList.Count && plateIngredients.SequenceEqual(requiredIngredientsList);
    }

    // Method to spawn the projectile at the throw point
    private void SpawnProjectile(GameObject projectilePrefab)
    {
        // Spawn the projectile at the throw point's position and rotation
        GameObject projectile = Instantiate(projectilePrefab, throwPoint.position, throwPoint.rotation);

        // Apply throw force to the projectile
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(throwPoint.forward * throwForce, ForceMode.Impulse);
        }
        else
        {
            Debug.LogError("Projectile prefab does not have a Rigidbody component!");
        }
    }
}
