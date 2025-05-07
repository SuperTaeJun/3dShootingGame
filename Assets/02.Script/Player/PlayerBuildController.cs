using UnityEngine;

public class PlayerBuildController : MonoBehaviour
{
    public BuildManager buildManager;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            buildManager.StartBuildMode(0); // Wall
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            buildManager.StartBuildMode(1); // Turret
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            buildManager.StartBuildMode(2); // Resource Generator
        }
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    buildManager.CancelBuildMode();
        //}
    }
}
