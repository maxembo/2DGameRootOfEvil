using UnityEngine;

public class Healer : UsableItem
{
    [SerializeField] private int healPoints;
    public int HealPoints => healPoints;

    /// <summary>
    /// Действует на применившего и на всех в радиусе.
    /// </summary>
    public override void PrimaryAction()
    {
        if (!_canUse) return;

        HealingCaster();

        if (_responseItems != null)
            foreach (var usable in _responseItems)
                usable.ResponseAction(gameObject);

        if (_destroyAfterUsing)
            Destroy(gameObject);

        StartCoroutine(CanUse());
    }

    /// <summary>
    /// Действует исключительно на применившего.
    /// </summary>
    public override void SecondaryAction()
    {
        if (!_canUse) return;

        HealingCaster();

        if (_destroyAfterUsing)
            Destroy(gameObject);

        StartCoroutine(CanUse());
    }

    /// <summary>
    /// Лечит заклинателя.
    /// </summary>
    private void HealingCaster()
    {
        if (
           transform.parent != null &&
           transform.parent.transform.TryGetComponent(out HealthProcessor healthProcessor)
           ) 
            healthProcessor.ResponseAction(gameObject);
    }
}