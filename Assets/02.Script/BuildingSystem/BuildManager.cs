using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public GameObject[] BuildingPrefabs;
    public LayerMask TerrainLayer;

    private GameObject _previewInstance;
    private int _selectedIndex = 0;

    void Update()
    {
        HandlePreview();
        HandleRotateInput(); 
    }

    public void StartBuildMode(int prefabIndex)
    {
        _selectedIndex = prefabIndex;


        if (_previewInstance != null) Destroy(_previewInstance);


        _previewInstance = Instantiate(BuildingPrefabs[_selectedIndex]);


        foreach (var col in _previewInstance.GetComponentsInChildren<Collider>()) col.enabled = false;
        var bo = _previewInstance.GetComponent<BuildableObject>();
        if (bo != null) Destroy(bo);


        _previewInstance.SetActive(false);

        SetPreviewMaterial(_previewInstance, new Color(1, 1, 1, 0.5f));

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void HandlePreview()
    {
        if (_previewInstance == null) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, 200f, TerrainLayer))
        {
            _previewInstance.SetActive(false);
            return;
        }


        Vector3 snappedPos = hit.point;


        _previewInstance.transform.position = snappedPos;
        _previewInstance.SetActive(true);


        bool canPlace = CanPlace(snappedPos) &&
                        CurrencyManager.Instance.GetCurrency(ECurrencyType.Gold)
                        >= BuildingPrefabs[_selectedIndex]
                          .GetComponent<BuildableObject>()
                          .BuildCost;
        SetPreviewMaterial(_previewInstance,canPlace ? Color.green * 0.5f : Color.red * 0.5f);


        if (canPlace && Input.GetMouseButtonDown(1))
        {
            var buildable = BuildingPrefabs[_selectedIndex].GetComponent<BuildableObject>();

  
            GameObject placed = Instantiate(
                BuildingPrefabs[_selectedIndex],
                snappedPos,
                Quaternion.identity
            );
            placed.tag = "Building";


            CurrencyManager.Instance.SpendCurrency(
                ECurrencyType.Gold,
                buildable.BuildCost
            );

            CancelBuildMode();
        }
    }

    private bool CanPlace(Vector3 pos)
    {
        float radius =  0.4f;
        foreach (var c in Physics.OverlapSphere(pos, radius))
            if (c.CompareTag("Building"))
                return false;
        return true;
    }

    public void CancelBuildMode()
    {
        if (_previewInstance != null)
        {
            Destroy(_previewInstance);
            _previewInstance = null;
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void HandleRotateInput()
    {
        if (_previewInstance != null && Input.GetKeyDown(KeyCode.Tab))
            _previewInstance.transform.Rotate(Vector3.up, 90f);
    }

    private void SetPreviewMaterial(GameObject go, Color col)
    {
        foreach (var r in go.GetComponentsInChildren<Renderer>())
            r.material.color = col;
    }
}
