using UnityEngine;

public class Wallet : MonoBehaviour
{
    [SerializeField] private int money = 100;

    public int Money => money;

    public void Add(int amount)
    {
        money += amount;
    }

    public bool CanAfford(int amount)
    {
        return money >= amount;
    }

    public bool TrySpend(int amount)
    {
        if (money < amount)
            return false;

        money -= amount;
        return true;
    }
}
