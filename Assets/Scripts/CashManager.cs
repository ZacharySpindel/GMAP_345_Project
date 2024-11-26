using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashManager : MonoBehaviour
{
    public static CashManager Instance { get; private set; }

    public float cash;

    private void Awake()
    {
        Instance = this;
    }

    public float GetCashValue()
    {
        return cash;
    }

    public void AddToCash(float value)
    {
        cash += value;
    }

    public void SubtractCash(float value)
    {
        cash -= value;
    }
}