using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum EUiType
{
    BombChargingBar,
    ReloadBar,
    CrossHair
}

public class PlayerUiManager : MonoBehaviour
{
    private Player _player;

    public static PlayerUiManager Instance;

    [Header("GameState")]
    public  TextMeshProUGUI ReadyText;
    public  TextMeshProUGUI RunText;
    public  TextMeshProUGUI OverText;


    [Header("Components")]
    public Slider           HealthBar;
    public Slider           StaminaBar;
    public Slider           BombChargingBar;
    public Slider           ReloadBar;
    public TextMeshProUGUI  BombText;
    public TextMeshProUGUI  BulletText;
    public Image            CrossHair;
    public Image[]          WeaponIcons;
    public TextMeshProUGUI GoldText;
    public TextMeshProUGUI DiamondText;
    [Header("UiElementss")]
    [SerializeField] private Dictionary<EUiType, GameObject> uiElements;

    [Header("Loading")]
    public Slider ProgressSlider;
    public TextMeshProUGUI ProgressText;
    public Image LoadingImage;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        }

        uiElements = new Dictionary<EUiType, GameObject>
        {
            { EUiType.BombChargingBar, BombChargingBar.gameObject },
            { EUiType.ReloadBar, ReloadBar.gameObject },
            {EUiType.CrossHair, CrossHair.gameObject }
        };

        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        GameManager.Instance.OnChangeGameStatea += OnChangeGameStatea;


    }
    private void Start()
    {

        PlayerWeaponController.OnWeaponChange += OnChanageWeaponUi;
        MyCamera.OnCameraTypeChanged += HandleCameraTypeChanged;
        CurrencyManager.Instance.OnChangedCurrency += RefreshCurrencyUi;

    }
    public void OnChanageWeaponUi(EWeaponType currentType)
    {
        foreach(var icon in WeaponIcons)
        {
            icon.gameObject.SetActive(false);
        }
        WeaponIcons[(int)currentType].gameObject.SetActive(true);
    }
    public void RefreshWeaponUi()
    {
        BombText.text = $"Bomb : {_player.CurrentBombNum} / {_player.WeaponController.CurrentWeapon.Data.MaxBombNum}";
        BulletText.text = $"Bullet : {_player.CurrentBulletNum} / {_player.WeaponController.CurrentWeapon.Data.MaxBulletNum}";
    }
    public void RefreshPlayer()
    {
        HealthBar.value = _player.CurrentHealth;
        StaminaBar.value = _player.CurrentStamina;
    }

    public void SetActiveUI(EUiType type, bool isActive)
    {
        if (uiElements.TryGetValue(type, out GameObject go))
        {
            go.SetActive(isActive);
        }
        else
        {
            Debug.LogWarning($"UIType {type}에 해당하는 UI 요소가 등록되지 않았습니다.");
        }
    }

    public void RefreshBombCharging(float curPower)
    {
        BombChargingBar.value = curPower;
    }
    public void RefreshReloadBar(float curReload)
    {
        ReloadBar.value = curReload;
    }

    private void HandleCameraTypeChanged(ECameraType cameraType)
    {
        bool showCrosshair = cameraType != ECameraType.QuarterView;
        SetActiveUI(EUiType.CrossHair, showCrosshair);
    }

      private void RefreshCurrencyUi()
    {
        GoldText.text = $"Gold : {CurrencyManager.Instance.GetCurrency(ECurrencyType.Gold)}";
        DiamondText.text = $"Diamond : {CurrencyManager.Instance.GetCurrency(ECurrencyType.Diamond)}";
    }

    public void OnChangeGameStatea(EGameState curState)
    {
    }

}
