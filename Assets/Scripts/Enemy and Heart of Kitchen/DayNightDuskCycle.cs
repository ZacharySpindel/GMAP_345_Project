using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;
using System.Collections;

public class DayNightDuskCycle : MonoBehaviour
{
    public Slider timeSlider;            // Reference to the UI Slider
    public GameObject[] spawners;        // Array of spawner GameObjects
    public RectTransform rotatingImage; // Reference to the image RectTransform to rotate

    public Image winImage; // Reference to the winning image


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
    public enum TimeOfDay { Day, Dusk, Night, Dawn }
    public TimeOfDay currentTimeOfDay;  // Current phase of the day-night cycle

    // Camera references
    public CinemachineVirtualCamera camera1;
    public CinemachineVirtualCamera camera2;
    public CinemachineVirtualCamera camera3;

    // Days allotted counter
    public int daysAllotted = 0;         // Variable to track the number of days allotted
    public TMP_Text daysAllottedText;     // Reference to the TMP Text to display the value

    // Reference to CashManager to get current cash value
    public CashManager cashManager;      // Reference to the CashManager

    // Reference to the UI Text to display Final Cash
    public TMP_Text finalCashText;
    public TMP_Text winText; // Reference to the "Win" text

    void Start()
    {
        // Initialize the starting phase
        currentTimeOfDay = TimeOfDay.Day;
        currentPhaseDuration = dayDuration;

        UpdateSlider();                  // Initialize the slider
        UpdateSpawners();                // Set spawners correctly for the starting phase
        SetCameraForPhase();             // Set the camera based on the starting phase
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

        // Check if the days allotted reach 5 and display the final cash
        if (daysAllotted >= 5)
        {
            DisplayFinalCash();
            DisplayWinText();
        }
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
        // Transition to the next phase
        switch (currentTimeOfDay)
        {
            case TimeOfDay.Day:
                currentTimeOfDay = TimeOfDay.Dusk;
                currentPhaseDuration = duskDuration;
                SetCameraForPhase(); // Update the camera for dusk
                break;
            case TimeOfDay.Dusk:
                currentTimeOfDay = TimeOfDay.Night;
                currentPhaseDuration = nightDuration;
                SetCameraForPhase(); // Update the camera for night
                break;
            case TimeOfDay.Night:
                currentTimeOfDay = TimeOfDay.Dawn;
                currentPhaseDuration = dawnDuration;
                SetCameraForPhase(); // Update the camera for dawn
                break;
            case TimeOfDay.Dawn:
                currentTimeOfDay = TimeOfDay.Day;
                currentPhaseDuration = dayDuration;
                SetCameraForPhase(); // Update the camera for day

                // Increment days once during the Dawn phase
                daysAllotted++;
                UpdateDaysText(); // Update the displayed days allotted text
                break;
        }

        // Reset timer and update spawners
        timer = 0f;
        UpdateSpawners();
    }

    private void UpdateSpawners()
    {
        // Activate spawners only during the Night phase
        bool isNight = currentTimeOfDay == TimeOfDay.Night;

        foreach (GameObject spawner in spawners)
        {
            spawner.SetActive(isNight); // Enable or disable the spawner
        }

        Debug.Log($"Current Time of Day: {currentTimeOfDay}. Spawners active: {isNight}");
    }

    private void UpdateSlider()
    {
        // Update the slider value based on the current phase progress
        float value = 1 - (timer / currentPhaseDuration);
        timeSlider.value = Mathf.Clamp01(value);
    }

    // Switch camera based on the time of day
    private void SetCameraForPhase()
    {
        switch (currentTimeOfDay)
        {
            case TimeOfDay.Day:
                camera1.gameObject.SetActive(true);
                camera2.gameObject.SetActive(false);
                camera3.gameObject.SetActive(false);
                break;
            case TimeOfDay.Night:
                camera1.gameObject.SetActive(false);
                camera2.gameObject.SetActive(true);
                camera3.gameObject.SetActive(false);
                break;
            case TimeOfDay.Dusk:
                camera1.gameObject.SetActive(false);
                camera2.gameObject.SetActive(false);
                camera3.gameObject.SetActive(true);
                break;
            case TimeOfDay.Dawn:
                camera1.gameObject.SetActive(false);
                camera2.gameObject.SetActive(false);
                camera3.gameObject.SetActive(false); // Optional: keep it off during dawn, or choose a specific camera
                break;
        }
    }

    // Update the displayed days allotted value
    private void UpdateDaysText()
    {
        if (daysAllottedText != null)
        {
            daysAllottedText.text = "Days Allotted: " + daysAllotted.ToString();
        }
    }

    // Display the final cash when the player wins
    private void DisplayFinalCash()
    {
        if (finalCashText != null && cashManager != null)
        {
            Debug.Log("Final Cash: " + cashManager.GetCashValue());
            finalCashText.text = "Final Cash: " + cashManager.GetCashValue().ToString();
        }
        else
        {
            Debug.LogWarning("CashManager or FinalCashText is not assigned!");
        }
    }

    // Display win text when the player reaches 5 days
    private void DisplayWinText()
    {
        if (winText != null)
        {
            winText.text = "You Win!";
            winText.gameObject.SetActive(true); // Make sure the Win text is visible
        }

        if (winImage != null)
        {
            winImage.gameObject.SetActive(true); // Make sure the winning image is visible
        }

    }
}
