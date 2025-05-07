using UnityEngine;
public enum BuildingType
{
    Wall,
    Turret,
    ResourceGenerator
}
public class BuildableObject : MonoBehaviour, IDamageable
{
    public string buildingName;
    public BuildingType buildingType;
    public int buildCost;
    public float maxHealth;

    private float currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void Repair(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    public void Upgrade(float healthIncrease, int additionalCost)
    {
        maxHealth += healthIncrease;
        buildCost += additionalCost;
    }

    private void DestroyBuilding()
    {
        Destroy(gameObject);
    }

    public void TakeDamage(Damage damage)
    {
        currentHealth -= damage.Value;

        if (currentHealth <= 0)
        {
            DestroyBuilding();
        }
    }
}
