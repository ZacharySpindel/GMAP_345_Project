using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeCompleted;
    public static DeliveryManager Instance { get; private set; }

    public string deliveryCounterID;

    [SerializeField] private RecipeListSO recipeListSO;

    [SerializeField] private DeliveryCounter deliveryCounter;

    private List<RecipeSO> waitingRecipeSOList;
    private List<string> tableIDBag; // Bag for holding table IDs

    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipesMax = 4;

    private void Awake()
    {
        waitingRecipeSOList = new List<RecipeSO>();
        tableIDBag = new List<string> { "triangle", "square", "circle", "star" }; // Initialize table IDs
        Debug.Log($"Initial Table ID Bag: {string.Join(", ", tableIDBag)}");
        Instance = this;
    }

    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;

            // Spawn a new recipe if there's room
            if (waitingRecipeSOList.Count < waitingRecipesMax)
            {
                SpawnRecipe();
            }
        }
    }

    private void SpawnRecipe()
    {
        if (tableIDBag.Count == 0)
        {
            Debug.LogWarning("No available table IDs! Cannot spawn more recipes.");
            return;
        }

        // Select a random recipe
        RecipeSO newRecipe = ScriptableObject.Instantiate(recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)]);

        // Randomly assign a table ID
        int randomIndex = UnityEngine.Random.Range(0, tableIDBag.Count);
        string assignedTableID = tableIDBag[randomIndex];
        tableIDBag.RemoveAt(randomIndex); // Remove the assigned ID from the bag
        newRecipe.tableID = assignedTableID;

        waitingRecipeSOList.Add(newRecipe);

        Debug.Log($"Spawned recipe: {newRecipe.recipeName} for table {newRecipe.tableID}");
        Debug.Log($"Table ID Bag after removal: {string.Join(", ", tableIDBag)}");

        OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        Debug.Log($"Attempting to deliver to table: {deliveryCounterID}");

        for (int i = 0; i < waitingRecipeSOList.Count; ++i)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

            Debug.Log($"Checking recipe: {waitingRecipeSO.recipeName} for table: {waitingRecipeSO.tableID}");

            if (IsRecipeMatch(plateKitchenObject, waitingRecipeSO))
            {
                CompleteRecipe(i, waitingRecipeSO);
                return;
            }
        }

        Debug.Log($"No matching recipe found for table: {deliveryCounterID}");
    }

    private bool IsRecipeMatch(PlateKitchenObject plate, RecipeSO recipe)
    {
        if (recipe.kitchenObjectSOList.Count != plate.GetKitchenObjectSOList().Count)
        {
            Debug.Log($"Ingredient count mismatch. Expected: {recipe.kitchenObjectSOList.Count}, Got: {plate.GetKitchenObjectSOList().Count}");
            return false;
        }

        foreach (KitchenObjectSO ingredient in recipe.kitchenObjectSOList)
        {
            if (!plate.GetKitchenObjectSOList().Contains(ingredient))
            {
                Debug.Log($"Ingredient {ingredient.name} not found on plate!");
                return false;
            }
        }

        if (recipe.tableID != deliveryCounterID)
        {
            Debug.Log($"Table ID mismatch. Expected: {recipe.tableID}, Got: {deliveryCounterID}");
            return false;
        }

        return true;
    }

    private void CompleteRecipe(int recipeIndex, RecipeSO recipe)
    {
        Debug.Log("Player delivered the correct recipe to the correct table!");

        // Reward the player
        CashManager.Instance.AddToCash(recipe.cashReward);
        PatronManager.Instance.PatronRecruitPoints(recipe.tableID, recipe.cashReward);
        Debug.Log($"Player received {recipe.cashReward} cash for delivering {recipe.recipeName}");

        // Remove recipe and return table ID to the bag
        waitingRecipeSOList.RemoveAt(recipeIndex);
        tableIDBag.Add(recipe.tableID); // Add table ID back to the bag
        Debug.Log($"Table ID {recipe.tableID} returned to bag.");
        Debug.Log($"Table ID Bag after addition: {string.Join(", ", tableIDBag)}");

        OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
    }

    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    }
}
