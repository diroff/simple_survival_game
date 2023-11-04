using TMPro;
using UnityEngine;

public class UIHealthPanel : MonoBehaviour
{
    [SerializeField] private HealthComponent _health;
    [SerializeField] private TextMeshProUGUI _text;

    private void OnEnable()
    {
        _health.OnHealthChanged += OnHealthChanged;
    }

    private void OnDisable()
    {
        _health.OnHealthChanged -= OnHealthChanged;
    }

    private void OnHealthChanged(int currentValue, int maxValue)
    {
        _text.text = $"{currentValue}/{maxValue}";
    }
}