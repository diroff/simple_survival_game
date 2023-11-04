using System;
using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private int _maxHealth;

    public UnityAction OnDamage;
    public UnityAction OnHeal;
    public UnityAction OnDie;

    public UnityAction<int, int> OnHealthChanged;

    private int _currentHealth;

    private void Start()
    {
        _currentHealth = _maxHealth;
        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
    }

    public void ModifyHealth(int healthDelta)
    {
        _currentHealth += healthDelta;

        if (healthDelta < 0)
            TakeDamage();

        if (healthDelta >= 0)
            Heal();

        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);

        if (_maxHealth <= 0)
            OnDie?.Invoke();
    }

    private void Heal()
    {
        OnHeal?.Invoke();
    }

    private void TakeDamage()
    {
        if (_currentHealth < 0)
            _currentHealth = 0;

        OnDamage?.Invoke();
    }
}