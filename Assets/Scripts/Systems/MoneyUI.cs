using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    private Wallet wallet;

    private void Awake()
    {
        if (text == null) text = GetComponent<TextMeshProUGUI>();
        wallet = FindFirstObjectByType<Wallet>();
    }

    private void Update()
    {
        if (wallet == null) return;
        text.text = $"$ {wallet.Money}";
    }
}
