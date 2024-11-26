using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPageOrder : MonoBehaviour
{
    private Button button;
    private Transform childElement;

    void Awake()
    {
        // Get the Button component on this GameObject
        button = GetComponent<Button>();

        // Find the first child of this Button GameObject
        if (transform.childCount > 0)
        {
            childElement = transform.GetChild(0);
        }
        else
        {
            Debug.LogError($"{gameObject.name} has no child element to bring to the front.");
        }
    }

    // void Start()
    // {
    //     // Register the OnClick event to only move this button's child
    //     if (button != null && childElement != null)
    //     {
    //         button.onClick.AddListener(() => BringChildToFront(childElement));
    //     }
    // }

    void BringChildToFront(Transform targetChild)
    {
        if (targetChild != null)
        {
            targetChild.SetAsLastSibling();
            Debug.Log($"{targetChild.name} has been moved to the front.");
        }
        else
        {
            Debug.LogError("Target child element not found.");
        }
    }

    void OnDestroy()
    {
        if (button != null)
        {
            button.onClick.RemoveListener(() => BringChildToFront(childElement));
        }
    }
}