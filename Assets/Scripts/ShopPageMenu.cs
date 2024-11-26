using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopPageMenu : MonoBehaviour
{
    // public GameObject element1, element2, element3;
    public List<GameObject> uiElements;
    public GameObject childElement1, childElement2, childElement3;
    // tab button activating menu
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            foreach (GameObject uiElement in uiElements)
            {
                if (uiElement != null)
                {
                    uiElement.SetActive(!uiElement.activeSelf);
                }
            }
        }
    }

    // Method to bring a specific child to the front
    public void BringToFront(GameObject childElement)
    {
        if (childElement != null)
        {
            childElement.transform.SetAsLastSibling();
            Debug.Log($"{childElement.name} has been moved to the front.");
        }
        else
        {
            Debug.LogError("Child element not found.");
        }
    }

}