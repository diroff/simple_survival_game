using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private float _power;
    [SerializeField] private int _damage;
    [SerializeField] private float _lifeTime;
    [SerializeField] private Collider2D _collider;

    private float _timeToDestroying = 0f;
    private Rigidbody2D _rigidbody;
    private HealthComponent _healthComponent;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (_timeToDestroying >= _lifeTime)
            Destroy(gameObject);

        _timeToDestroying += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            return;

        _healthComponent = collision.GetComponent<HealthComponent>();

        if (_healthComponent == null)
            return;

        _healthComponent.ModifyHealth(-_damage);
        Destroy(gameObject);
    }

    public void IgnoreCollisionWith(Collider2D otherCollider)
    {
        Physics2D.IgnoreCollision(_collider, otherCollider);
    }

    public void Move(Vector3 direction)
    {
        _rigidbody.AddForce(direction * _power, ForceMode2D.Impulse);
    }
}