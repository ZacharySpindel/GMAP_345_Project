using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter
{

    public event EventHandler OnPlayerGrabObject;
    
    [SerializeField] private KitchenObjectSO kitchenObjectSO;


    public override void Interact(Player player)
    {
       if (!player.HasKitchenObject()) //if player isn't carrying anything
        {
            //Debug.Log("Interact!");
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);


            OnPlayerGrabObject?.Invoke(this, EventArgs.Empty); //you've grabbed this object. event args empty is a formality I'm pretty sure
                

            //Debug.Log(kitchenObjectTransform.GetComponent<KitchenObject>().GetKitchenObjectSO().objectName);
        }



       
    }





}
