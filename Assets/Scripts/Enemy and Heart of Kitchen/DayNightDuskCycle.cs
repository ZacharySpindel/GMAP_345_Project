using UnityEngine;
using UnityEngine.UI;
using TMPro; // Add this for TextMeshPro

public class DayNightDuskCycle : MonoBehaviour
{
    public Slider timeSlider;            // Reference to the UI Slider
    public GameObject[] spawners;        // Array of spawner GameObjects
    public RectTransform rotatingImage; // Reference to the image RectTransform to rotate
    public TextMeshProUGUI dayCounterText; // Reference to TextMeshPro for day count
    public TextMeshProUGUI winMessageText; // Reference to TextMeshPro for win message
    public TextMeshProUGUI finalCashText;  // Reference to TextMeshPro for final cash

    // Durations for each phase
    public float dayDuration = 90f;
    public float duskDuration = 30f;
    public float nightDuration = 60f;
    public float dawnDuration = 2f;

    // Animation curves for each phase
    public AnimationCurve dayCurve;
    public AnimationCurve duskCurve;
    public AnimationCurve nightCurve;
    public AnimationCurve dawnCurve;

    private float timer = 0f;           // Timer to track the current phase progress
    private float currentPhaseDuration; // Current phase duration
    private int currentDay = 1;         // Track the current day
    private const int maxDays = 5;      // Maximum number of days for win condition
    public enum TimeOfDay { Day, Dusk, Night, Dawn }
    public TimeOfDay currentTimeOfDay;  // Current phase of the day-night cycle

    void Start()
    {
        // Initialize the starting phase
        currentTimeOfDay = TimeOfDay.Day;
        currentPhaseDuration = dayDuration;

        UpdateSlider();                  // Initialize the slider
        UpdateSpawners();                // Set spawners correctly for the starting phase

        UpdateDayCounter();              // Update the day counter UI
        winMessageText.gameObject.SetActive(false); // Hide win message at the start
        finalCashText.gameObject.SetActive(false);  // Hide final cash message at the start
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Rotate the image continuously
        RotateImage();

        // Transition to the next phase if the timer exceeds the phase duration
        if (timer >= currentPhaseDuration)
        {
            TransitionToNextPhase();
        }

        UpdateSlider();
    }

    private void RotateImage()
    {
        if (rotatingImage != null)
        {
            // Get the appropriate animation curve for the current phase
            AnimationCurve currentCurve = GetCurrentAnimationCurve();

            if (currentCurve != null)
            {
                // Evaluate the speed from the curve based on the normalized timer
                float normalizedTime = timer / currentPhaseDuration;
                float rotationSpeed = currentCurve.Evaluate(normalizedTime);

                // Rotate the image around its Z-axis
                rotatingImage.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
            }
        }
    }

    private AnimationCurve GetCurrentAnimationCurve()
    {
        return currentTimeOfDay switch
        {
            TimeOfDay.Day => dayCurve,
            TimeOfDay.Dusk => duskCurve,
            TimeOfDay.Night => nightCurve,
            TimeOfDay.Dawn => dawnCurve,
            _ => null,
        };
    }

    private void TransitionToNextPhase()
    {
        switch (currentTimeOfDay)
        {
            case TimeOfDay.Day:
                currentTimeOfDay = TimeOfDay.Dusk;
                currentPhaseDuration = duskDuration;
                Debug.Log("Switching to Dusk");

                // Increment the day count when transitioning from Day to Dusk
                IncrementDayCount();
                break;

            case TimeOfDay.Dusk:
                currentTimeOfDay = TimeOfDay.Night;
                currentPhaseDuration = nightDuration;
                Debug.Log("Switching to Night");
                break;

            case TimeOfDay.Night:
                currentTimeOfDay = TimeOfDay.Dawn;
                currentPhaseDuration = dawnDuration;
                Debug.Log("Switching to Dawn");
                break;

            case TimeOfDay.Dawn:
                currentTimeOfDay = TimeOfDay.Day;
                currentPhaseDuration = dayDuration;
                Debug.Log("Switching to Day");
                break;
        }

        // Reset timer and update spawners
        timer = 0f;
        UpdateSpawners();
    }

    private void IncrementDayCount()
    {
        currentDay++;
        UpdateDayCounter();

        if (currentDay > maxDays)
        {
            TriggerWinCondition();
        }
    }

    private void TriggerWinCondition()
    {
        Time.timeScale = 0; // Pause the game
        winMessageText.text = "You Win!";
        winMessageText.gameObject.SetActive(true);

        int finalCash = CashManager.Instance.GetCashValue();
        finalCashText.text = $"Final Cash: ${finalCash}";
        finalCashText.gameObject.SetActive(true);
    }

    private void UpdateDayCounter()
    {
        dayCounterText.text = $"Days: {currentDay}/{maxDays}";
    }

    private void UpdateSpawners()
    {
        // Activate spawners only during the Night phase
        bool isNight = currentTimeOfDay == TimeOfDay.Night;

        foreach (GameObject spawner in spawners)
        {
            spawner.SetActive(isNight); // Enable or disable the spawner
            spawner.GetComponent<EnemySpawner>()?.SetActive(isNight); // Resume spawning if enabled
        }

        Debug.Log($"Current Time of Day: {currentTimeOfDay}. Spawners active: {isNight}");
    }

    private void UpdateSlider()
    {
        // Update the slider value based on the current phase progress
        float value = 1 - (timer / currentPhaseDuration);
        timeSlider.value = Mathf.Clamp01(value);
    }
}
