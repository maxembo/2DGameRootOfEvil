using JetBrains.Annotations;
using UnityEngine;

public class QuestAction : MonoBehaviour, IResponsable
{
    [Tooltip("Привязка к квесту (Оставьте пустым если нет)")] [SerializeField]
    private ItemQuest _quest;

    [Tooltip("Реагировать на объект (Оставьте пустым если нет)")] [SerializeField]
    private UsableItem _responseItem;

    [Tooltip("Смена спрайта на новый (Оставьте пустым если нет)")] [SerializeField]
    private Sprite _newSprite;

    [SerializeField] [CanBeNull] private Hint _hint;

    [Tooltip("Награда за действие (Оставьте пустым если нет)")] [SerializeField]
    private GameObject _reward;
    
    [Tooltip("Кол-во наград за действие (Оставьте пустым если нет)"), SerializeField, Min(0)]
    private int _rewardCount;

    [Tooltip("Позиция спавна награды (Оставьте пустым если нет)")] [SerializeField]
    private Vector2 _rewardPosition;

    [Tooltip("Нужна ли засчитывать квестовое действие (Оставьте пустым если нет)")] [SerializeField]
    private bool _reduceActions = true;

    [Tooltip("Отключен компонент до начала квеста.")] [SerializeField]
    private bool _sleepy;

    [Tooltip("Скрыт ли объект до начала квеста.")] [SerializeField]
    private bool _isHidden;

    [Tooltip("Выполняется при попадании в триггер")] [SerializeField]
    protected bool _passiveExecution;

    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _collider;


    private void Start() => PreInitialize();

    private void PreInitialize()
    {
        SetNullableFields();

        if (_isHidden)
        {
            _collider.enabled = false;

            if (_hint != null)
                _hint.gameObject.SetActive(false);
        }

        if (_sleepy)
            enabled = false;

        if (_quest != null) EventHandler.OnQuestStart.AddListener(CheckStartingQuest);
        else Initialize();
    }

    private void Initialize()
    {
        enabled = true;

        _collider.enabled = true;

        if (_hint != null)
            _hint.gameObject.SetActive(true);
    }

    private void SetNullableFields()
    {
        _collider = GetComponent<BoxCollider2D>();

        if (_spriteRenderer == null)
            _spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    public void CheckStartingQuest(Quest quest)
    {
        if (quest == _quest)
            Initialize();
    }

    public void ResponseAction(GameObject g)
    {
        if (g.TryGetComponent(out UsableItem item) && _responseItem != null && CompareItemsName(_responseItem, item))
            CompleteAction();
    }

    private void CompleteAction()
    {
        if (!enabled) return;

        if (_reduceActions)
            _quest.ReduceAction();

        GiveReward();

        ChangeSprite();
    }

    private void GiveReward()
    {
        if (_reward != null && _rewardCount > 0)
        {
            Instantiate(_reward, GetRewardPos(), Quaternion.identity);

            _rewardCount--;
        }
    }

    private void ChangeSprite()
    {
        if (_newSprite != null) _spriteRenderer.sprite = _newSprite;
        else _spriteRenderer.enabled = false;
    }

    private bool CompareItemsName(UsableItem first, UsableItem second) =>
        first.nameItem == second.nameItem;

    private Vector2 GetRewardPos() => (Vector2)transform.position + _rewardPosition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_passiveExecution)
            return;

        if (collision.TryGetComponent(out UsableItem item))
            CompleteAction();
    }

    // private void OnTriggerExit2D(Collider2D collision)
    // {
    //     if (!_passiveExecution)
    //         return;
    //
    //     if (collision.TryGetComponent(out UsableItem item))
    //         CompleteAction();
    // }
}