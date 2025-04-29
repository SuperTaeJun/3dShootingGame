using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyUiController : MonoBehaviour
{
    public Slider HealthBar;

    public void SetActiveHealthBar(bool active)
    {
        HealthBar.gameObject.SetActive(active);
    }
    public void RefreshPlayer(float currentHealth)
    {
        HealthBar.value = currentHealth;
    }
    private void Update()
    {
        HealthBar.gameObject.transform.forward = Camera.main.transform.forward;
    }
}
