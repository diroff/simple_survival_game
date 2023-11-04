using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthPanel : MonoBehaviour
{
    [SerializeField] private HealthComponent _health;
    [SerializeField] private TextMeshProUGUI _textField;
    [SerializeField] private Slider _slider;

    private void Awake()
    {
        if(_health == null)
            _health = GetComponentInParent<HealthComponent>();
    }

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
        _textField.text = $"{currentValue}/{maxValue}";

        _slider.value = (float)currentValue / maxValue;
    }
}