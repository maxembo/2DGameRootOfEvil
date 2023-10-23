using Core.Health;
using System.Collections;
using Scripts.Character.Classes;
using Scripts.Core.Global;
using UnityEngine;

public class HealthProcessor : MonoBehaviour, IHealth, IResponsable
{
    [Header("Required Components")] [SerializeField]
    private SpriteRenderer _spriteRenderer;

    [Header("HealthBar")] 
    [SerializeField] private HealthBar _healthBar;

    [Header("Parameters")] 
    [SerializeField] private int _maxHitPoints;
    [SerializeField] private int _currentHitPoints;
    [SerializeField] [Min(1)] private float _coefDefense;

    private Health _health;

    private void Start() => Initialize();

    private void Initialize()
    {
        if (_spriteRenderer == null)
            _spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();

        _health = new Health(_maxHitPoints, _coefDefense);
        
        ChangeHealthBar();
    }

    public void ResponseAction(GameObject g)
    {
        if (g.TryGetComponent(out Weapon weapon))
            TakeDamage(weapon.Damage);

        if (g.TryGetComponent(out Healer healer))
            TakeHeal(healer.HealPoints);
    }

    public void TakeDamage(float damage)
    {
        _health.TakeDamage(damage);

        ChangeHealthBar();

        StartCoroutine(ChangeColor(GlobalConstants.DamageColor));

        CheckDeath();
    }

    public void TakeHeal(float heal)
    {
        if (!CanHealing())
            return;

        _health.TakeHeal(heal);

        ChangeHealthBar();

        StartCoroutine(ChangeColor(GlobalConstants.HealColor));
    }

    public void ChangeHealthBar()
    {
        _currentHitPoints = _health.HitPoints;
        
        if (_healthBar != null)
            _healthBar.SetCurrentHealth(_currentHitPoints * 100 / _maxHitPoints);
    }
    
    private IEnumerator ChangeColor(Color color)
    {
        SetColor(color);

        yield return new WaitForSeconds(GlobalConstants.TimeChangeColor);

        SetColor(GlobalConstants.DefaultColor);
    }

    private void SetColor(Color color) => _spriteRenderer.color = color;

    private bool CanHealing() => _currentHitPoints < _maxHitPoints;

    private void CheckDeath()
    {
        if (_currentHitPoints <= 0 && transform.TryGetComponent(out Character character))
            character.Death();
    }
}