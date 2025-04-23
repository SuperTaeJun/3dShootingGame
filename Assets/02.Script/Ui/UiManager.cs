using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;

    public Slider           StaminaBar;
    public TextMeshProUGUI  BombText;
    public Slider           BombChargingBar;
    public TextMeshProUGUI  BulletText;
    public Slider           ReloadBar;
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
            Debug.LogWarning("싱글톤 여러개임");
        }
    }

    public void RefreshStamina(float curStamina)
    {
        StaminaBar.value = curStamina;
    }
    public void RefreshBomb(int curBombNum, int maxBombNum)
    {
        BombText.text = $"Bomb : {curBombNum} / {maxBombNum}";
    }
    public void RefreshBombCharging(float curPower)
    {
        BombChargingBar.value = curPower;
    }
    public void SetActiveBombChargingBar(bool isActive)
    {
        BombChargingBar.gameObject.SetActive(isActive);
    }
    public void RefreshBullet(int curBulletNum, int maxBulletNum)
    {
        BulletText.text = $"Bullet : {curBulletNum} / {maxBulletNum}";
    }
    public void SetActiveReloadBar(bool isActive)
    {
        ReloadBar.gameObject.SetActive(isActive);
    }
    public void RefreshReloadBar(float curReload)
    {
        ReloadBar.value = curReload;
    }
}
