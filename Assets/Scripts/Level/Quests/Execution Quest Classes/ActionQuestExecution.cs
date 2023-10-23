using JetBrains.Annotations;
using Scripts.Level.Quests.Quest_Classes;
using UnityEngine;

namespace Scripts.Level.Quests.Execution_Quest_Classes
{
    public class ActionQuestExecution : QuestExecution, IResponsable
    {
        [Tooltip("Привязка к квесту (Оставьте пустым если нет)")]
        [SerializeField] [CanBeNull] private PerformActionQuest _quest;

        [Tooltip("Предмет для использования")]
        [SerializeField] private UsableItem _responseItem;
                
        [Tooltip("Нужна ли засчитывать квестовое действие")] 
        [SerializeField] protected bool _reduceAction = true;
        
        protected override void PreInitialize()
        {
            base.PreInitialize();
            
            if (_quest != null) EventHandler.OnQuestProgressing.AddListener(CheckStartingQuest);
            else Initialize();
        }
        
        protected override void CheckStartingQuest(Quest quest)
        {
            if (_quest != null && _quest == quest) Initialize();
        }

        public void ResponseAction(GameObject g)
        {
            if (g.TryGetComponent(out UsableItem item) && CompareItemsName(_responseItem, item))
                PerformAction();
        }
        
        private bool CompareItemsName(UsableItem first, UsableItem second) => first.nameItem == second.nameItem;

        protected override void PerformAction()
        {
            base.PerformAction();

            if (_quest != null && _reduceAction) _quest.PerformAction();
        }

        protected override void CheckCondition()
        {
            if (_quest != null) _quest.CheckCondition();

            if (_oneUse) _canUse = false;
        }
    }
}
