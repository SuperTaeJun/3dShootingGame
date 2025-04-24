using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

    [Header("Components")]
    public Slider           StaminaBar;
    public Slider           BombChargingBar;
    public Slider           ReloadBar;
    public TextMeshProUGUI  BombText;
    public TextMeshProUGUI  BulletText;
    public Image            CrossHair;

    [Header("UiElementss")]
    [SerializeField] private Dictionary<EUiType, GameObject> uiElements;

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
    }
    private void Start()
    {
        MyCamera.OnCameraTypeChanged += HandleCameraTypeChanged;
    }
    public void RefreshWeaponUi()
    {
        BombText.text = $"Bomb : {_player.CurrentBombNum} / {_player.PlayerData.MaxBombNum}";
        BulletText.text = $"Bullet : {_player.CurrentBulletNum} / {_player.PlayerData.MaxBulletNum}";
    }
    public void RefreshPlayer()
    {
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
}
