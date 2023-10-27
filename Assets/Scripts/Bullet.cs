using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [SerializeField] private Collider2D _collider;

    private AmmoItemInfo _ammoInfo;

    private int _damage;

    private float _lifeTime;
    private float _power;
    private float _timeToDestroying = 0f;

    private Rigidbody2D _rigidbody;
    private HealthComponent _healthComponent;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void SetupAmmoInfo()
    {
        _lifeTime = _ammoInfo.LifeTime;
        _damage += _ammoInfo.Damage;
    }

    public void SetAmmoInfo(AmmoItemInfo info)
    {
        _ammoInfo = info;
    }

    public void SetDamage(int damage)
    {
        _damage = damage;
    }

    public void SetPower(float power)
    {
        _power = power;
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