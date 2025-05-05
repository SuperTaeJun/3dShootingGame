using System;
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

    public static Action<EWeaponType> OnWeaponChange;

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
        // 키보드 숫자키 입력
        if (Input.GetKeyDown(KeyCode.Alpha1))
            ChangeWeaponByIndex(0);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            ChangeWeaponByIndex(1);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            ChangeWeaponByIndex(2);

        // 마우스 휠 입력
        float scroll = Input.mouseScrollDelta.y;
        if (scroll != 0)
        {
            int weaponCount = System.Enum.GetNames(typeof(EWeaponType)).Length;
            int currentIndex = (int)CurrentWeaponType;

            if (scroll > 0)
                currentIndex = (currentIndex + 1) % weaponCount; // 다음 무기
            else
                currentIndex = (currentIndex - 1 + weaponCount) % weaponCount; // 이전 무기

            ChangeWeaponByIndex(currentIndex);
        }
    }

    private void ChangeWeaponByIndex(int index)
    {
        EWeaponType newType = (EWeaponType)index;
        if (newType != CurrentWeaponType)
        {
            CurrentWeaponType = newType;
            SetCurrentWeapon();
            OnWeaponChange.Invoke(CurrentWeaponType);
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
