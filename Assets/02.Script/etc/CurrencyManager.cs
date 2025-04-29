using System;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    [SerializeField] private Currency _currency;

    public  Action OnChangedCurrency;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        _currency = new Currency();
    }

    public int GetCurrency(ECurrencyType type)
    {
        return type switch
        {
            ECurrencyType.Gold => _currency.Gold,
            ECurrencyType.Diamond => _currency.Diamond,
            _ => 0,
        };
    }

    public void AddCurrency(ECurrencyType type, int amount)
    {
        _currency.Add(type, amount);
        OnChangedCurrency.Invoke();
    }

    public void SpendCurrency(ECurrencyType type, int amount)
    {
        _currency.Spend(type, amount);
        OnChangedCurrency.Invoke();
    }

}
