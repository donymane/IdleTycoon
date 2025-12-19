using UnityEngine;

public class HotelButtons : MonoBehaviour
{
    [SerializeField] private HotelManager hotel;

    [Header("Tuning")]
    public int priceStep = 100;
    public int repairCost = 500;

    private void Awake()
    {
        if (hotel == null) hotel = FindFirstObjectByType<HotelManager>();
    }

    public void OnPricePlus()
    {
        if (hotel == null) return;
        hotel.ChangePrice(+priceStep);
    }

    public void OnPriceMinus()
    {
        if (hotel == null) return;
        hotel.ChangePrice(-priceStep);
    }

    public void OnRepair()
    {
        if (hotel == null) return;
        hotel.Repair(repairCost, 100f);
    }
}
