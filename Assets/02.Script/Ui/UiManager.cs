using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;

    public Slider StaminaBar;

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

}
