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
            Debug.LogWarning("ΩÃ±€≈Ê ø©∑Ø∞≥¿”");
        }
    }

    public void RefreshStamina(float curStamina)
    {
        StaminaBar.value = curStamina;
    }

}
