using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabInstantiator : MonoBehaviour
{
    // Public variable to assign the prefab in the Inspector
    public GameObject prefabToInstantiate;

    // Public variable to set the build point in the Inspector
    public Transform buildPoint;

    void Update()
    {
        // Check if the "B" key is pressed
        if (Input.GetKeyDown(KeyCode.B))
        {
            // Instantiate the prefab at the build point's position and rotation
            Instantiate(prefabToInstantiate, buildPoint.position, buildPoint.rotation);
        }
    }
}