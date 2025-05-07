using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public GameObject[] buildingPrefabs;

    public float gridSize = 2f;
    public LayerMask terrainLayer;

    private GameObject previewInstance;
    private int selectedIndex = 0;

    void Update()
    {
        HandlePreview();
        HandleRotateInput(); 
    }

    public void StartBuildMode(int prefabIndex)
    {
        selectedIndex = prefabIndex;


        if (previewInstance != null) Destroy(previewInstance);


        previewInstance = Instantiate(buildingPrefabs[selectedIndex]);


        foreach (var col in previewInstance.GetComponentsInChildren<Collider>()) col.enabled = false;
        var bo = previewInstance.GetComponent<BuildableObject>();
        if (bo != null) Destroy(bo);


        previewInstance.SetActive(false);

        SetPreviewMaterial(previewInstance, new Color(1, 1, 1, 0.5f));

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void HandlePreview()
    {
        if (previewInstance == null) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, 200f, terrainLayer))
        {
            previewInstance.SetActive(false);
            return;
        }

 
        Vector3 snapped = new Vector3(
            Mathf.Round(hit.point.x / gridSize) * gridSize,
            hit.point.y,
            Mathf.Round(hit.point.z / gridSize) * gridSize
        );

        previewInstance.transform.position = snapped;
        previewInstance.SetActive(true);


        bool canPlace = CanPlace(snapped) &&
                        CurrencyManager.Instance.GetCurrency(ECurrencyType.Gold)
                        >= buildingPrefabs[selectedIndex]
                          .GetComponent<BuildableObject>()
                          .buildCost;
        SetPreviewMaterial(previewInstance,canPlace ? Color.green * 0.5f : Color.red * 0.5f);


        if (canPlace && Input.GetMouseButtonDown(1))
        {
            var buildable = buildingPrefabs[selectedIndex].GetComponent<BuildableObject>();

  
            GameObject placed = Instantiate(
                buildingPrefabs[selectedIndex],
                snapped,
                Quaternion.identity
            );
            placed.tag = "Building";


            CurrencyManager.Instance.SpendCurrency(
                ECurrencyType.Gold,
                buildable.buildCost
            );

            CancelBuildMode();
        }
    }

    private bool CanPlace(Vector3 pos)
    {
        float radius = gridSize * 0.4f;
        foreach (var c in Physics.OverlapSphere(pos, radius))
            if (c.CompareTag("Building"))
                return false;
        return true;
    }

    public void CancelBuildMode()
    {
        if (previewInstance != null)
        {
            Destroy(previewInstance);
            previewInstance = null;
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void HandleRotateInput()
    {
        if (previewInstance != null && Input.GetKeyDown(KeyCode.Tab))
            previewInstance.transform.Rotate(Vector3.up, 90f);
    }

    private void SetPreviewMaterial(GameObject go, Color col)
    {
        foreach (var r in go.GetComponentsInChildren<Renderer>())
            r.material.color = col;
    }
}
