using UnityEngine;

public class BuildSlot : MonoBehaviour
{
    [SerializeField] private Transform snapPoint;
    [SerializeField] private bool occupied;

    private Building placedBuilding;

    public bool IsOccupied => occupied;
    public Building PlacedBuilding => placedBuilding;

    public bool TryPlace(GameObject prefab, BuildingData data)
    {
        if (occupied || prefab == null) return false;

        var pos = snapPoint != null ? snapPoint.position : transform.position;

        var rot = Quaternion.Euler(0f, 0f, 0f);

        var go = Instantiate(prefab, pos, rot);

        placedBuilding = go.GetComponent<Building>();
        if (placedBuilding != null)
            placedBuilding.Setup(data);

        occupied = true;
        return true;
    }
}
