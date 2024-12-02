using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class YTopDownMode : MonoBehaviour
{
    // Public variables to assign the Cinemachine virtual cameras from the Unity Inspector
    public CinemachineVirtualCamera camera1;
    public CinemachineVirtualCamera camera2;
    public CinemachineVirtualCamera camera3;

    // Integer to keep track of the current camera state
    private int cameraState = 0; // 0: All on, 1: Camera 1 off, 2: Cameras 1 & 2 off

    // Reference to DayNightDuskCycle for automatic camera changes
    public DayNightDuskCycle dayNightCycle;

    void Update()
    {
        // Check if the "Y" key is pressed (to toggle cameras manually)
        if (Input.GetKeyDown(KeyCode.Y))
        {
            ToggleCameras();
        }

        // Automatically change cameras based on the time of day in the DayNightDuskCycle
        if (dayNightCycle != null)
        {
            AutoSwitchCameras(dayNightCycle.currentTimeOfDay);
        }
    }

    // Method to toggle the cameras based on the current state (user input)
    void ToggleCameras()
    {
        // Increment the camera state (cycle through 0, 1, 2)
        cameraState = (cameraState + 1) % 3;

        switch (cameraState)
        {
            case 0:
                // All cameras active (default state)
                camera1.gameObject.SetActive(true);
                camera2.gameObject.SetActive(true);
                camera3.gameObject.SetActive(true);
                break;
            case 1:
                // Camera 1 off, Camera 2 and 3 on
                camera1.gameObject.SetActive(false);
                camera2.gameObject.SetActive(true);
                camera3.gameObject.SetActive(true);
                break;
            case 2:
                // Cameras 1 and 2 off, only Camera 3 on
                camera1.gameObject.SetActive(false);
                camera2.gameObject.SetActive(false);
                camera3.gameObject.SetActive(true);
                break;
        }
    }

    // Method to automatically switch cameras based on the time of day
    public void AutoSwitchCameras(DayNightDuskCycle.TimeOfDay timeOfDay)
    {
        switch (timeOfDay)
        {
            case DayNightDuskCycle.TimeOfDay.Day:
                // Camera 1 on, others off during Day phase
                camera1.gameObject.SetActive(true);
                camera2.gameObject.SetActive(false);
                camera3.gameObject.SetActive(false);
                break;
            case DayNightDuskCycle.TimeOfDay.Dusk:
                // Camera 3 on, others off during Dusk phase
                camera1.gameObject.SetActive(false);
                camera2.gameObject.SetActive(false);
                camera3.gameObject.SetActive(true);
                break;
            case DayNightDuskCycle.TimeOfDay.Night:
                // Camera 2 on, others off during Night phase
                camera1.gameObject.SetActive(false);
                camera2.gameObject.SetActive(true);
                camera3.gameObject.SetActive(false);
                break;
            case DayNightDuskCycle.TimeOfDay.Dawn:
                // Camera 1 on, others off during Dawn phase
                camera1.gameObject.SetActive(true);
                camera2.gameObject.SetActive(false);
                camera3.gameObject.SetActive(false);
                break;
        }
    }
}
