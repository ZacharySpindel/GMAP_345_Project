using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{

    public string selfID;

    public override void Interact(Player player)
    {
        if(player.HasKitchenObject())
        {
            DeliveryManager.Instance.deliveryCounterID = GetSelfID();
            if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) 
                //only accepts plates
            {

                DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);
                player.GetKitchenObject().DestroySelf(); 
            }
        }
    }
    public string GetSelfID()
    {
        return selfID;
    }

}
