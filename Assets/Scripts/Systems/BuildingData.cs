using UnityEngine;

public enum BuildingType
{
    Room,
    Cafe,
    Pool
}

[System.Serializable]
public class BuildingData
{
    public BuildingType type;
    public int cost;
    public float profitPerSecond;

    public BuildingData(BuildingType t, int c, float pps)
    {
        type = t; cost = c; profitPerSecond = pps;
    }
}
