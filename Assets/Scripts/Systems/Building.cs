using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private BuildingType type;
    [SerializeField] private float profitPerSecond;

    public BuildingType Type => type;
    public float ProfitPerSecond => profitPerSecond;

    public void Setup(BuildingData data)
    {
        type = data.type;
        profitPerSecond = data.profitPerSecond;
    }
}
