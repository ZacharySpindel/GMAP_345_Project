using System;
using UnityEngine;

public class CashManager : MonoBehaviour
{
    public static CashManager Instance { get; private set; }

    private int currentCash;

    [SerializeField] private TMPro.TextMeshProUGUI cashText;  // Reference to the TextMeshProUGUI for displaying cash

    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Initialize cash and update the UI display
        currentCash = 0;
        UpdateCashDisplay();
    }

    // Add cash to the current balance
    public void AddToCash(int amount)
    {
        currentCash += amount;
        Debug.Log("Cash Added: " + amount + ", New Cash: " + currentCash); // Debug log to track cash changes
        UpdateCashDisplay();
    }

    public void SubtractToCash(int amount)
    {
        currentCash -= amount;
        Debug.Log("Cash Added: " + amount + ", New Cash: " + currentCash); // Debug log to track cash changes
        UpdateCashDisplay();
    }

    // Retrieve the current cash value
    public int GetCashValue()
    {
        return currentCash;
    }

    // Update the TextMeshProUGUI to display the current cash value
    private void UpdateCashDisplay()
    {
        if (cashText != null)
        {
            cashText.text = "Cash: $" + currentCash.ToString();
        }
    }
}
