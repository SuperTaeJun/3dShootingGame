using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public enum EWeaponType
{
    Rifle,
    Shotgun,
    Soward
}

public class PlayerWeaponController : MonoBehaviour
{

    EWeaponType CurrentWeaponType;
    [SerializeField] private WeaponBase[] _weapons;

    public WeaponBase CurrentWeapon;


    private void Awake()
    {
        CurrentWeaponType = EWeaponType.Rifle;
        CurrentWeapon = _weapons[(int)CurrentWeaponType];
    }

    private void Start()
    {
    }
    private void Update()
    {
        if (GameManager.Instance.CurrentState != EGameState.Run) return;

        WeaponChangeHandler();
        CurrentWeapon.Attack();
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
            CurrentWeaponType = EWeaponType.Shotgun;
            SetCurrentWeapon();
        }
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            CurrentWeaponType = EWeaponType.Soward;
            SetCurrentWeapon();
        }
    }

    private void SetCurrentWeapon()
    {
        foreach (var weapon in _weapons)
        {
            weapon.gameObject.SetActive(false);
        }
        CurrentWeapon = _weapons[(int)CurrentWeaponType];
        CurrentWeapon.gameObject.SetActive(true);
    }
}
