using UnityEngine;

[CreateAssetMenu(fileName = "SO_WeaponData", menuName = "Scriptable Objects/SO_WeaponData")]
public class SO_WeaponData : ScriptableObject
{
    [Header("Weapon Settings")]
    public int MaxBombNum = 3;
    public int MaxBulletNum = 50;
    public float FireRate = 0.2f;
    public float ReloadTime = 2f;
    public int Damage = 5;
}
