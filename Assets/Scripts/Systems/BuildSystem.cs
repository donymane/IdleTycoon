using TMPro;
using UnityEngine;

public class BuildSystem : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask slotMask = ~0;

    [Header("Prefabs")]
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private GameObject cafePrefab;
    [SerializeField] private GameObject poolPrefab;

    [Header("UI")]
    [SerializeField] private TMP_Text selectedLabel;

    private Wallet wallet;
    private HotelManager hotel;

    private BuildingType selectedType = BuildingType.Room;

    private void Awake()
    {
        if (cam == null) cam = Camera.main;
        wallet = FindFirstObjectByType<Wallet>();
        hotel = FindFirstObjectByType<HotelManager>();
    }

    private void Start()
    {
        RefreshSelectedLabel();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            TryBuildAtMouse();
    }

    public void SelectRoom()
    {
        selectedType = BuildingType.Room;
        RefreshSelectedLabel();
    }

    public void SelectCafe()
    {
        selectedType = BuildingType.Cafe;
        RefreshSelectedLabel();
    }

    public void SelectPool()
    {
        selectedType = BuildingType.Pool;
        RefreshSelectedLabel();
    }

    // <<< бнр рн, врн рш опняхк >>>
    public void SelectNext()
    {
        selectedType = selectedType switch
        {
            BuildingType.Room => BuildingType.Cafe,
            BuildingType.Cafe => BuildingType.Pool,
            _ => BuildingType.Room
        };

        RefreshSelectedLabel();
    }

    private void RefreshSelectedLabel()
    {
        if (selectedLabel == null) return;

        var data = GetData(selectedType);
        selectedLabel.text = $"{selectedType} (${data.cost})";
    }

    private void TryBuildAtMouse()
    {
        if (cam == null || wallet == null) return;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 200f, slotMask))
        {
            var slot = hit.collider.GetComponentInParent<BuildSlot>();
            if (slot == null || slot.IsOccupied) return;

            var data = GetData(selectedType);
            if (wallet.Money < data.cost) return;

            var prefab = GetPrefab(selectedType);
            if (prefab == null) return;

            bool placed = slot.TryPlace(prefab, data);
            if (placed)
                wallet.TrySpend(data.cost);
        }
    }

    private GameObject GetPrefab(BuildingType t)
    {
        return t switch
        {
            BuildingType.Room => roomPrefab,
            BuildingType.Cafe => cafePrefab,
            BuildingType.Pool => poolPrefab,
            _ => roomPrefab
        };
    }

    private BuildingData GetData(BuildingType t)
    {
        return t switch
        {
            BuildingType.Room => new BuildingData(t, 50, 1.5f),
            BuildingType.Cafe => new BuildingData(t, 120, 4.0f),
            BuildingType.Pool => new BuildingData(t, 200, 6.5f),
            _ => new BuildingData(t, 50, 1.5f)
        };
    }
}
