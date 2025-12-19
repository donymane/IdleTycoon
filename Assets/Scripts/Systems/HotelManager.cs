using UnityEngine;

public class HotelManager : MonoBehaviour
{
    [Header("Core stats (0..100)")]
    [Range(0, 100)] public float marketing = 20f;   // реклама
    [Range(0, 100)] public float condition = 80f;   // ремонт/состояние

    [Header("Price per stay")]
    public int price = 1200;

    [Header("Balancing")]
    public int idealPrice = 1200;     // “нормальная” цена для текущего уровня отеля
    public int tolerance = 1500;      // насколько гости терпят завышение

    [Header("Condition decay (idle)")]
    public float conditionLossPerSec = 0.02f;       // пассивный износ
    public float badConditionThreshold = 40f;       // порог "плохо"
    public float marketingDropWhenBadPerSec = 0.05f; // падение маркетинга при плохом ремонте (пассивно)

    [Header("Guest impact")]
    public float conditionLossPerAcceptedGuest = 1.5f; // износ за заселившегося гостя
    public float marketingPenaltyOnBadGuest = 0.5f;    // штраф к маркетингу, если заселили при плохом ремонте

    // Интервал спавна гостей: marketing 0..100 => 10..2 сек
    public float SpawnIntervalSeconds
    {
        get
        {
            float t = Mathf.Clamp01(marketing / 100f);
            return Mathf.Lerp(10f, 2f, t);
        }
    }

    // Шанс заселения: зависит от цены и ремонта
    public float AcceptChance
    {
        get
        {
            float c = Mathf.Clamp01(condition / 100f);

            // цена: идеально -> 1, выше идеала -> падает
            float priceFactor = 1f - Mathf.Max(0f, (price - idealPrice) / (float)tolerance);
            priceFactor = Mathf.Clamp01(priceFactor);

            // ремонт влияет мягко (не обнуляет резко)
            float conditionFactor = 0.5f + 0.5f * c;

            return Mathf.Clamp01(priceFactor * conditionFactor);
        }
    }

    private void Update()
    {
        // пассивный износ
        condition = Mathf.Clamp(condition - conditionLossPerSec * Time.deltaTime, 0f, 100f);

        // если ремонт плохой — падает маркетинг (репутация)
        if (condition < badConditionThreshold)
        {
            marketing = Mathf.Clamp(marketing - marketingDropWhenBadPerSec * Time.deltaTime, 0f, 100f);
        }
        RecalcProfit();
        EarnIdle();
    }

    public void ChangePrice(int delta)
    {
        price = Mathf.Clamp(price + delta, 100, 10000);
    }

    public void OnGuestArrived(bool accepted)
    {
        if (!accepted) return;

        // износ от заселения
        condition = Mathf.Clamp(condition - conditionLossPerAcceptedGuest, 0f, 100f);

        // если заселили при плохом ремонте — репутация страдает сильнее
        if (condition < badConditionThreshold)
        {
            marketing = Mathf.Clamp(marketing - marketingPenaltyOnBadGuest, 0f, 100f);
        }
    }

    public void Repair(int cost, float restoreTo = 100f)
    {
        var wallet = FindFirstObjectByType<Wallet>();
        if (wallet == null) return;

        if (!wallet.TrySpend(cost)) return;

        condition = Mathf.Clamp(restoreTo, 0f, 100f);
    }

    public void OnGuestRejected()
    {
        float penalty = (condition < badConditionThreshold) ? 1.5f : 0.5f;
        marketing = Mathf.Clamp(marketing - penalty, 0f, 100f);
    }

    public float TotalProfitPerSecond { get; private set; }

    private float profitTimer;

    private void EarnIdle()
    {
        var wallet = FindFirstObjectByType<Wallet>();
        if (wallet == null) return;

        profitTimer += Time.deltaTime;
        if (profitTimer >= 1f)
        {
            profitTimer = 0f;
            wallet.Add(Mathf.RoundToInt(TotalProfitPerSecond));
        }
    }

    private void RecalcProfit()
    {
        float sum = 0f;
        var slots = FindObjectsByType<BuildSlot>(FindObjectsSortMode.None);
        foreach (var s in slots)
        {
            if (s != null && s.IsOccupied && s.PlacedBuilding != null)
                sum += s.PlacedBuilding.ProfitPerSecond;
        }
        TotalProfitPerSecond = sum;
    }


}
