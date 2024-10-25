using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlateIconsSingleUI : MonoBehaviour
{

    [SerializeField] private Image image;

    public void SetKitchenobjectSO(KitchenObjectSO kitchenObjecSO)
    {
        image.sprite = kitchenObjecSO.sprite;
    }




}
