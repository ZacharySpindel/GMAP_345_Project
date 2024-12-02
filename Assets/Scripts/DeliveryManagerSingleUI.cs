using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryManagerSingleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeNameText;  // Text component to display recipe name and cash value
    [SerializeField] private Transform iconContainer;         // Container to hold recipe ingredient icons
    [SerializeField] private Transform iconTemplate;          // Template for ingredient icons
    [SerializeField] private Image tableIDImage;              // Image component to display the corresponding tableID sprite

    [SerializeField] private Sprite triangleSprite;           // Sprite for 'triangle' tableID
    [SerializeField] private Sprite squareSprite;             // Sprite for 'square' tableID
    [SerializeField] private Sprite circleSprite;             // Sprite for 'circle' tableID
    [SerializeField] private Sprite starSprite;               // Sprite for 'star' tableID

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);  // Disable the template initially
    }

    // Method to update the UI with the recipe's details
    public void SetRecipeSO(RecipeSO recipeSO)
    {
        // Update recipe name to include its cash value
        recipeNameText.text = $"{recipeSO.recipeName} (${GetRecipeCashValue(recipeSO)})";

        // Clear any existing icons
        foreach (Transform child in iconContainer)
        {
            if (child == iconTemplate) continue;  // Skip the template itself
            Destroy(child.gameObject);  // Destroy any previous icon children
        }

        // Instantiate icons for each kitchen object in the recipe
        foreach (KitchenObjectSO kitchenObjectSO in recipeSO.kitchenObjectSOList)
        {
            Transform iconTransform = Instantiate(iconTemplate, iconContainer);
            iconTransform.gameObject.SetActive(true);
            iconTransform.GetComponent<Image>().sprite = kitchenObjectSO.sprite;  // Set the sprite for each ingredient
        }

        // Set the tableID sprite based on the recipe's tableID
        SetTableIDSprite(recipeSO.tableID);
    }

    // Method to set the table sprite based on the tableID
    private void SetTableIDSprite(string tableID)
    {
        switch (tableID)
        {
            case "triangle":
                tableIDImage.sprite = triangleSprite;
                break;
            case "square":
                tableIDImage.sprite = squareSprite;
                break;
            case "circle":
                tableIDImage.sprite = circleSprite;
                break;
            case "star":
                tableIDImage.sprite = starSprite;
                break;
            // Add additional cases here if you have more tableIDs (e.g., diamond, pentagon, etc.)
            default:
                Debug.LogWarning("Unknown tableID: " + tableID);
                break;
        }
    }

    // Method to get the cash value for each recipe
    private int GetRecipeCashValue(RecipeSO recipeSO)
    {
        Debug.Log($"Getting cash value for recipe: {recipeSO.recipeName}");

        return recipeSO.recipeName switch
        {
            "Salad" => 10,
            "Burger" => 15,
            "CheeseBurger" => 20,
            "MegaBurger" => 30,
            _ => LogAndReturnDefault(recipeSO.recipeName)
        };
    }

    private int LogAndReturnDefault(string recipeName)
    {
        Debug.LogWarning($"Recipe name '{recipeName}' does not match any known recipes. Returning default value of 0.");
        return 0;
    }

}
