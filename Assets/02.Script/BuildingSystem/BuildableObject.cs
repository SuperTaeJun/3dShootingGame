using UnityEngine;
public enum BuildingType
{
    Turret
}
public class BuildableObject : MonoBehaviour, IDamageable
{
    public BuildingType BuildingType;
    public int BuildCost;
    public float MaxHealth;

    private float _currentHealth;

    private void Awake()
    {
        _currentHealth = MaxHealth;
    }

    public void Repair(float amount)
    {
        _currentHealth = Mathf.Min(_currentHealth + amount, MaxHealth);
    }

    public void Upgrade(float healthIncrease, int additionalCost)
    {
        MaxHealth += healthIncrease;
        BuildCost += additionalCost;
    }

    private void DestroyBuilding()
    {
        Destroy(gameObject);
    }

    public void TakeDamage(Damage damage)
    {
        _currentHealth -= damage.Value;

        if (_currentHealth <= 0)
        {
            DestroyBuilding();
        }
    }
}
