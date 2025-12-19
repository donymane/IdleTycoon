using TMPro;
using UnityEngine;

public class HotelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI marketingText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI conditionText;

    private HotelManager hotel;

    private void Awake()
    {
        hotel = FindFirstObjectByType<HotelManager>();
    }

    private void Update()
    {
        if (hotel == null) return;

        if (marketingText) marketingText.text = $"Marketing: {hotel.marketing:0}";
        if (priceText) priceText.text = $"Price: {hotel.price}";
        if (conditionText) conditionText.text = $"Condition: {hotel.condition:0}";
    }
}
