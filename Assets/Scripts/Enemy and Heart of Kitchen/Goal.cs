using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour
{
    public int maxHealth = 100;        // Maximum health of the goal
    public int health;                 // Current health of the goal
    public GameObject gameOverText;    // Reference to the game-over text UI
    public Slider healthSlider;        // Reference to the UI Slider for health

    private void Start()
    {
        health = maxHealth;            // Initialize goal's health to maximum at start
        gameOverText.SetActive(false); // Ensure game-over text is hidden initially

        // Set up the health slider's max value and initial value
        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        AudioManager.Instance.PlaySFX("Loss HP");

        // Update the slider's value to reflect the current health
        healthSlider.value = health;

        // Check if health has reached zero
        if (health <= 0)
        {
            GameOverGoalManager.Instance.GameOver();
        }
    }
}
