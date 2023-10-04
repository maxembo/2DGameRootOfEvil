using UnityEngine;

public class Weapon : UsableItem
{
    [SerializeField] private int _damage;
    private int _damageFactor = 1;

    public int Damage => _damage * _damageFactor;

    public int DamageFactor
    {
        set => _damageFactor = value;
    }

    public override void PrimaryAction()
    {
        if (!_canUse) return;

        if (_responseItems != null)
            foreach (var usable in _responseItems)
                usable.ResponseAction(gameObject);
        
        if (_destroyAfterUsing)
            Destroy(gameObject);

        StartCoroutine(CanUse());
    }
}