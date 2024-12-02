using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

[CreateAssetMenu()]

public class RecipeSO : ScriptableObject
{
    public List<KitchenObjectSO> kitchenObjectSOList;
    public string recipeName;

    public string tableID;

    public float itemRPs;
    public int cashReward;  // Cash reward for delivering this recipe (e.g., 10 for Salad)


}
