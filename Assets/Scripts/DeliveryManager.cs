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
    private float spawnRecipeTimerMax =  4f;
    private int waitingRecipesMax = 4;


    private void Awake()
    {
        waitingRecipeSOList = new List<RecipeSO>(); //initilize list
        Instance = this;
    }

    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;
        if(spawnRecipeTimer <= 0f )
        {
            spawnRecipeTimer = spawnRecipeTimerMax;

            //spawn recipe
            if(waitingRecipeSOList.Count < waitingRecipesMax ) 
            {
                RecipeSO waitingRecipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
               
                waitingRecipeSOList.Add(waitingRecipeSO);
                
                waitingRecipeSO.tableID = RandomTableID();
                Debug.Log(waitingRecipeSO.recipeName + ' ' + waitingRecipeSO.tableID);


                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
            


        }
    }

    private string RandomTableID()
    {
        return tableID[UnityEngine.Random.Range(0, 4)]; // change 2nd condition to 8 after testing
    }


    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        for(int i = 0; i < waitingRecipeSOList.Count; ++i) 
        { 
            RecipeSO waitingRecipeSO = waitingRecipeSOList[i];


            if(waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList().Count)
            {
                //has same nuber of ingredients, as a first check

                bool plateContentsMatchesRecipe = true;

                foreach(KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
                {
                    //cycling through all ingredients in recipe
                    bool ingredientFound = false;
                    foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.GetKitchenObjectSOList())
                    {
                        //cycling through all ingredients in plate
                        if(plateKitchenObjectSO == recipeKitchenObjectSO)
                        {
                            //ingredient matches!
                            ingredientFound = true;
                            break;
                        }
                    }
                    if(!ingredientFound)
                    {
                        //This recipe ingredient was not found on the plate
                        plateContentsMatchesRecipe = false;
                    }
                }

                if (waitingRecipeSO.tableID == deliveryCounterID) //plateContentsMatchesRecipe
                {
                    Debug.Log(waitingRecipeSO.tableID);
                    //player delivers to right table
                    if (plateContentsMatchesRecipe)
                    {
                        //player delivered correct recipe
                        Debug.Log("player delivered correct recipe");
                        waitingRecipeSOList.RemoveAt(i);
                        //give cash upon giving correct recipe
                        CashManager.Instance.AddToCash(5);
                        Debug.Log(CashManager.Instance.GetCashValue());
                        OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
                        return;
                    }
                }

            }
        }
        //no matches found
        //player did not deliver a correct reccipe
        Debug.Log("player did not deliver correct recipe " + deliveryCounterID);

    }


    public List<RecipeSO> GetWaitingRecipeSOList()
    {
        return waitingRecipeSOList;
    }



}
