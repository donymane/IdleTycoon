using UnityEngine;

public class MoneyGenerator : MonoBehaviour
{
    [SerializeField] private int incomePerTick = 1;
    [SerializeField] private float tickSeconds = 1f;

    private Wallet wallet;
    private float timer;

    public void Init(Wallet w) => wallet = w;
    private void Start()
    {
        if (wallet == null) wallet = FindFirstObjectByType<Wallet>();
    }

    private void Update()
    {
        if (wallet == null) return;

        timer += Time.deltaTime;
        if (timer >= tickSeconds)
        {
            timer = 0f;
            wallet.Add(incomePerTick);
        }
    }
}
