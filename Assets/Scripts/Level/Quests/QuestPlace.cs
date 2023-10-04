using UnityEngine;

public class QuestPlace : MonoBehaviour
{
    [Tooltip("Привязка к квесту (Оставьте пустым если нет)")] [SerializeField]
    private ItemQuest _quest;

    [Tooltip("Отключен компонент до начала квеста.")] [SerializeField]
    private bool _sleepy;

    [Tooltip("Выполняется при попадании в триггер")] [SerializeField]
    protected bool _passiveExecution;

    private UsableItem _questItem;

    private void Start() => PreInitialize();

    private void PreInitialize()
    {
        if (_sleepy)
            enabled = false;

        if (_quest != null) EventHandler.OnQuestStart.AddListener(CheckStartingQuest);
        else Initialize();
    }

    private void Initialize()
    {
        enabled = true;
    }

    public void CheckStartingQuest(Quest quest)
    {
        if (quest == _quest && _quest.questItem.nameItem != null)
        {
            _questItem = _quest.questItem;

            Initialize();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_passiveExecution)
            return;

        if (collision.TryGetComponent(out UsableItem item))
        {
            _quest.AddItem(item);
            _quest.CheckStage();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!_passiveExecution)
            return;

        if (collision.TryGetComponent(out UsableItem item))
        {
            _quest.RemoveItem(item);
            _quest.CheckStage();
        }
    }
}