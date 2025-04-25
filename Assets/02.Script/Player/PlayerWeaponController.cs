using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public enum EWeaponType
{
    Rifle,
    Soward
}

public class PlayerWeaponController : MonoBehaviour
{

    EWeaponType CurrentWeaponType;
    public WeaponBase[] Weapons;
    private Player _player;



    private void Awake()
    {
        _player = GetComponent<Player>();
        CurrentWeaponType = EWeaponType.Rifle;
    }

    private void Start()
    {
    }
    private void Update()
    {
        WeaponChangeHandler();
        Weapons[(int)CurrentWeaponType].Attack();
    }
    private void WeaponChangeHandler()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            CurrentWeaponType = EWeaponType.Rifle;
            SetCurrentWeapon();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CurrentWeaponType = EWeaponType.Soward;
            SetCurrentWeapon();
        }
    }

    private void SetCurrentWeapon()
    {
        foreach (var weapon in Weapons)
        {
            weapon.gameObject.SetActive(false);
        }
        Weapons[(int)CurrentWeaponType].gameObject.SetActive(true);
    }
}
