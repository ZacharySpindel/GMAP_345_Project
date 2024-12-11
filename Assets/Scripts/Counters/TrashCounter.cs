using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        if(player.HasKitchenObject()) //if  player has a kitchen object
        {
            
           player.GetKitchenObject().DestroySelf(); //get that object and destroy it! Shadow Realm!!!
           AudioManager.Instance.PlaySFX("Throwing Trash");
        }
    }
}
