using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

public class QuestAction : MonoBehaviour, IUsable
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

    [Tooltip("Позиция спавна награды (Оставьте пустым если нет)")] [SerializeField]
    private Vector2 _rewardPosition;

    [FormerlySerializedAs("_isCounted")]
    [Tooltip("Нужна ли засчитывать квестовое действие (Оставьте пустым если нет)")]
    [SerializeField]
    private bool _reduceActions = true;

    [Tooltip("Отключен компонент до начала квеста.")] [SerializeField]
    private bool _sleepy;

    [Tooltip("Скрыт ли объект до начала квеста.")] [SerializeField]
    private bool _isHidden;

    [Tooltip("Выполняется при попадании в триггер")] [SerializeField]
    protected bool _passiveExecution;

    private UsableItem _questItem;
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
        if (quest == _quest && _quest.questItem.nameItem != null)
        {
            _questItem = _quest.questItem;

            Initialize();
        }
    }

    public void ResponseAction(GameObject g)
    {
        if (g.TryGetComponent(out UsableItem item) && CompareItemsName(_responseItem, item)) ;
        CompleteAction();
    }

    private void CompleteAction()
    {
        if (!enabled) return;

        if (_reduceActions)
            _quest.ReduceAction();

        if (_reward != null)
            Instantiate
            (
                _reward,
                GetRewardPos(),
                Quaternion.identity
            );

        if (_newSprite != null) _spriteRenderer.sprite = _newSprite;
        else _spriteRenderer.enabled = false;
    }

    private bool CheckAndGetItem(GameObject g, out UsableItem item)
    {
        if (g.TryGetComponent(out item) && CompareItemsName(_questItem, item)) ;
        return true;
    }

    private bool CompareItemsName(UsableItem first, UsableItem second) =>
        first.nameItem == second.nameItem || first == null || second == null;

    private Vector2 GetRewardPos() => (Vector2)transform.position + _rewardPosition;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_passiveExecution)
            return;

        if (CheckAndGetItem(collision.gameObject, out UsableItem item))
        {
            _quest.AddItem(item);
            CompleteAction();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!_passiveExecution)
            return;

        if (CheckAndGetItem(collision.gameObject, out UsableItem item))
        {
            _quest.RemoveItem(item);
            CompleteAction();
        }
    }
}