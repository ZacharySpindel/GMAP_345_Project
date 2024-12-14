using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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

    private List<string> tableID = new List<string> { "triangle", "square", "circle", "star" };    // add diamond, pentagon, moon, and cross after testing

    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipesMax = 4;


    private void Awake()
    {
        waitingRecipeSOList = new List<RecipeSO>(); // Initialize list
        Instance = this;
    }

    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;

            // Spawn recipe if there is room for more
            if (waitingRecipeSOList.Count < waitingRecipesMax)
            {
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];

                waitingRecipeSOList.Add(waitingRecipeSO);

                waitingRecipeSO.tableID = RandomTableID();
                Debug.Log($"Spawned recipe: {waitingRecipeSO.recipeName} for table {waitingRecipeSO.tableID}");

                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private string RandomTableID()
    {
        return tableID[UnityEngine.Random.Range(0, 4)]; // Change to 8 after testing
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        Debug.Log($"Attempting to deliver to table: {deliveryCounterID}");

        for (int i = 0; i < waitingRecipeSOList.Count; ++i)
        {
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

            Debug.Log($"Checking recipe: {waitingRecipeSO.recipeName} for table: {waitingRecipeSO.tableID}");

            // Check if the recipe matches the plate's ingredients
            if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                bool plateContentsMatchesRecipe = true;

                foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
                {
                    bool ingredientFound = false;

                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        if (plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            ingredientFound = true;
                            break;
                        }
                    }

                    if (!ingredientFound)
                    {
                        plateContentsMatchesRecipe = false;
                        Debug.Log($"Ingredient {recipeKitchenObjectSO.name} not found on plate!");
                        break;
                    }
                }

                if (waitingRecipeSO.tableID == deliveryCounterID)
                {
                    Debug.Log($"Table ID matches: {waitingRecipeSO.tableID}");

                    if (plateContentsMatchesRecipe)
                    {
                        Debug.Log("Player delivered the correct recipe to the correct table!");

                        // Reward the player based on the cashReward of the recipe
                        CashManager.Instance.AddToCash(waitingRecipeSO.cashReward);
                        PatronManager.Instance.PatronRecruitPoints(waitingRecipeSO.tableID, waitingRecipeSO.cashReward);
                        Debug.Log($"Player received {waitingRecipeSO.cashReward} cash for delivering {waitingRecipeSO.recipeName}");

                        waitingRecipeSOList.RemoveAt(i);
                        OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                        return;
                    }
                    else
                    {
                        Debug.Log("Ingredients did not match the recipe.");
                    }
                }
                else
                {
                    Debug.Log($"Table ID mismatch. Expected: {waitingRecipeSO.tableID}, Got: {deliveryCounterID}");
                }
            }
            else
            {
                Debug.Log($"Ingredient count mismatch. Expected: {waitingRecipeSO.kitchenObjectSOList.Count}, Got: {plateKitchenObject.GetKitchenObjectSOList().Count}");
            }
        }

        Debug.Log($"No matching recipe found for table: {deliveryCounterID}");
    }



    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    }
}
