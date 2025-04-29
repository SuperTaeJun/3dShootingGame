using UnityEngine;
public enum ECurrencyType
{
    Gold,
    Diamond,
}

[System.Serializable]
public class Currency : MonoBehaviour
{
    [SerializeField] private int _gold;
    [SerializeField] private int _diamond;

    public int Gold => _gold;
    public int Diamond => _diamond;

    public Currency()
    {
        _gold = 0;
        _diamond = 0;
    }

    public void Add(ECurrencyType type, int amount)
    {
        switch (type)
        {
            case ECurrencyType.Gold:
                _gold += amount;
                break;
            case ECurrencyType.Diamond:
                _diamond += amount;
                break;
        }
    }

    public void Spend(ECurrencyType type, int amount)
    {
        switch (type)
        {
            case ECurrencyType.Gold:
                _gold = Mathf.Max(0, _gold - amount);
                break;
            case ECurrencyType.Diamond:
                _diamond = Mathf.Max(0, _diamond - amount);
                break;
        }
    }
}
