using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableItem : MonoBehaviour
{
    public string nameItem;

    [Tooltip("Одноразовые предметы исчезают после использования")] [SerializeField]
    protected bool _destroyAfterUsing;

    [Tooltip("Выполняется непрерывно")] [SerializeField]
    protected bool _continuousExecution;

    [Tooltip("Выполняется при попадании в триггер")] [SerializeField]
    protected bool _passiveExecution;

    [Tooltip("Задержка использования")] [SerializeField]
    protected float _delayUsing = 0.1f;

    [Tooltip("Удаление компонента у предмета, после использования")] [SerializeField]
    protected bool _disableComponent;

    protected bool _canUse = true;
    protected List<IResponsable> _responseItems = new();

    public virtual void Update()
    {
        if (_continuousExecution) PassiveAction();
    }

    public virtual void PrimaryAction()
    {
        if (!enabled) return;

        foreach (var usable in _responseItems)
            usable.ResponseAction(gameObject);

        if (_disableComponent)
            enabled = false;

        if (_destroyAfterUsing)
            Destroy(gameObject);
    }

    public virtual void SecondaryAction() { }

    public void PassiveAction()
    {
        if (_passiveExecution) PrimaryAction();
    }

    protected IEnumerator CanUse()
    {
        _canUse = false;
        yield return new WaitForSeconds(_delayUsing);
        _canUse = true;
    }

    private void AddToList(Collider2D collision)
    {
        if (!collision.TryGetComponent(out IResponsable usable)) return;
        if (!_responseItems.Contains(usable))
            _responseItems.Add(usable);
    }

    private void RemoveFromList(Collider2D collision)
    {
        if (!collision.TryGetComponent(out IResponsable usable)) return;
        if (_responseItems.Contains(usable))
            _responseItems.Remove(usable);
    }

    private void OnTriggerStay2D(Collider2D collision) => AddToList(collision);

    private void OnTriggerExit2D(Collider2D collision) => RemoveFromList(collision);
}