using JetBrains.Annotations;
using Scripts.Level.Quests.Quest_Classes;
using UnityEngine;

namespace Scripts.Level.Quests.Execution_Quest_Classes
{
    public abstract class QuestExecution : MonoBehaviour
    {
        [Tooltip("Выключить после одного раза")]
        [SerializeField] protected bool _oneUse;
        
        [Tooltip("Sprite Renderer")]
        [SerializeField] protected SpriteRenderer _spriteRenderer;
        
        [Tooltip("Смена спрайта на новый (Оставьте пустым если нет)")] 
        [SerializeField] protected Sprite _newSprite;

        [Tooltip("Подсказка (Оставьте пустым если нет)")] 
        [SerializeField] [CanBeNull] protected Hint _hint;

        [Tooltip("Отключен компонент до начала квеста")]
        [SerializeField] protected bool _sleepy;

        [Tooltip("Скрыт ли объект до начала квеста")] 
        [SerializeField] protected bool _isHidden;
        
        [Tooltip("Спавнер награды/объектов квеста")] 
        [SerializeField] protected RewardSpawner _rewardSpawner;
        
        protected BoxCollider2D _trigger;
        protected bool _canUse = true;
        
        protected virtual void Start() => PreInitialize();
        
        protected virtual void PreInitialize()
        {
            SetNullableFields();

            if (_sleepy) enabled = false;
            
            if (_isHidden)
            {
                _trigger.enabled = false;

                if (_hint != null) _hint.gameObject.SetActive(false);
            }
        }

        public void Initialize()
        {
            enabled = true;

            _trigger.enabled = true;

            if (_hint != null) _hint.gameObject.SetActive(true);
        }
        
        protected abstract void CheckStartingQuest(Quest quest);
        
        protected abstract void CheckCondition();
        
        protected virtual void PerformAction()
        {
            if (!enabled) return;

            CheckCondition();
            
            GiveReward();

            ChangeOrDisableSprite();
        }

        protected void GiveReward()
        {
            if (_rewardSpawner != null) _rewardSpawner.GiveReward();
        }

        protected void SetNullableFields()
        {
            _trigger = GetComponent<BoxCollider2D>();

            if (_spriteRenderer == null) _spriteRenderer = transform.GetComponentInChildren<SpriteRenderer>();
        }
        
        public void ChangeOrDisableSprite()
        {
            if (_spriteRenderer == null) return;
            
            if (_newSprite != null) _spriteRenderer.sprite = _newSprite;
            else _spriteRenderer.enabled = false;
        }
    }
}