using JetBrains.Annotations;
using Scripts.Level.Quests.Quest_Classes;
using UnityEngine;

namespace Scripts.Level.Quests.Execution_Quest_Classes
{
    public class PlaceExecution : QuestExecution
    {
        [Tooltip("Привязка к квесту (Оставьте пустым если нет)")]
        [SerializeField] [CanBeNull] private DeliveryQuest _quest;
        
        protected override void PreInitialize()
        {
            base.PreInitialize();
            
            if (_quest != null) EventHandler.OnQuestProgressing.AddListener(CheckStartingQuest);
            else Initialize();
        }

        protected override void CheckCondition()
        {
            if (_quest != null) _quest.CheckCondition();

            if (_oneUse) _canUse = false;
        }

        protected override void CheckStartingQuest(Quest quest)
        {
            if (_quest != null && _quest == quest) Initialize();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out UsableItem item))
            {
                AddItem(item);

                PerformAction();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out UsableItem item))
            {
                RemoveItem(item);

                PerformAction();
            }
        }

        private void AddItem(UsableItem item)
        {
            if (_quest != null) _quest.AddItem(item);
        }

        private void RemoveItem(UsableItem item)         
        {
            if (_quest != null) _quest.RemoveItem(item);
        }
    }
}