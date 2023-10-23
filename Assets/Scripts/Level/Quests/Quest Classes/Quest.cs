using Scripts.Level.Quests.Dialogue_Classes;
using Scripts.Level.Quests.Execution_Quest_Classes;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Scripts.Level.Quests.Quest_Classes
{
    public abstract class Quest : Conversation
    {
        [Header("Quest Stage")]
        public QuestStages stage = QuestStages.Passed;

        [Header("Quest")]
        public string questName;

        [SerializeField] private string _description;

        [Tooltip("Нужно ли засчитывать квест")]
        [SerializeField] private bool _mainQuest = true;
        
        [Header("Base Replicas")]
        [TextArea(2, 4)] public string givingReplica;
        [TextArea(2, 4)] public string[] noDoneReplicas;
        [TextArea(2, 4)] public string completeReplica;

        [Space, Header("Give Item")]
        [Tooltip("Дать квестовый предмет и на какой стадии (Оставьте поле пустым, если нет)")]
        [SerializeField] private QuestStage<UsableItem> _reward;
        
        [Space]
        [SerializeField] private QuestStruct<Quest>[] _companionableQuests;
        [SerializeField] private DialogueStruct[] _companionableDialogs;

        private QuestStages _startStage;

        public enum QuestStages
        {
            NotAvailable,
            NotStarted,
            Progressing,
            Completed,
            Passed
        };

        public override void Initialize()
        {
            SaveData();
            
            stage = QuestStages.NotStarted;
        }

        public override void Talk() => CheckStage();

        public override bool IsAvailable() => 
                stage != QuestStages.NotAvailable && stage != QuestStages.Passed
                && IsRequiredConversationsPassed();

        public override string GetDescription() => _description;

        protected abstract bool SomeCondition();
        
        public void CheckCondition()
        {
            if (SomeCondition() && stage == QuestStages.Progressing) SetStage(QuestStages.Completed);
            else if (!SomeCondition() && stage == QuestStages.Completed) SetStage(QuestStages.Progressing);
        }

        public void SetStage(QuestStages stage)
        {
            this.stage = stage;

            CanGivingReward(_reward);
            CheckOptQuests(_companionableQuests, stage);
            CheckOptDialogs(_companionableDialogs, stage);

            switch (stage)
            {
                case QuestStages.Progressing:
                    EventHandler.OnQuestProgressing?.Invoke(this);
                    break;
                case QuestStages.Completed:
                    EventHandler.OnQuestCompleted?.Invoke(this);
                    break;
                case QuestStages.Passed:
                    PassConversation();
                    break;
            }
        }

        protected void CanGivingReward(QuestStage<UsableItem> reward)
        {
            if (stage == reward.achievableStage)
                Questor.GiveReward(reward.obj);
        }
        
        protected void CheckOptQuests(QuestStruct<Quest>[] quests, QuestStages stage)
        {
            foreach (var quest in quests)
                if (quest.questStage.achievableStage == stage)
                    quest.questStage.obj.SetStage(quest.assignableStage);
        }

        protected void CheckOptDialogs(DialogueStruct[] dialogs, QuestStages stage)
        {
            foreach (var dialog in dialogs)
                if (dialog.achievableStage == stage)
                    dialog.companionableDialog.SetAvailability(dialog.availability);
        }

        public void CheckStage()
        {
            CheckCondition();

            switch (stage)
            {
                case QuestStages.NotStarted:
                    StartQuest();
                    break;
                case QuestStages.Progressing:
                    ProgressingQuest();
                    break;
                case QuestStages.Completed:
                    CompleteQuest();
                    break;
            }
        }

        protected void StartQuest()
        {
            EventHandler.OnReplicaSay2?.Invoke(Questor, givingReplica);

            SetStage(QuestStages.Progressing);
        }

        protected void ProgressingQuest()
        {
            EventHandler.OnReplicaSay2?.Invoke(Questor, NoDoneReplica());

            SetStage(QuestStages.Progressing);
        }

        protected void CompleteQuest()
        {
            Questor.SetSmileSprite();

            EventHandler.OnReplicaSay2?.Invoke(Questor, completeReplica);

            SetStage(QuestStages.Passed);
        }

        public override void PassConversation()
        {
            UnsubscribeListeners();

            EventHandler.OnConversationPassed?.Invoke(this);////////
            
            if (_mainQuest)
                EventHandler.OnQuestPassed2?.Invoke(this);
        }

        protected void UnsubscribeListeners()
        {
            EventHandler.OnQuestProgressing.RemoveAllListeners();
            EventHandler.OnQuestCompleted.RemoveAllListeners();
            EventHandler.OnConversationPassed.RemoveAllListeners();
            EventHandler.OnQuestPassed2.RemoveAllListeners();
            EventHandler.OnDialogPassed2.RemoveAllListeners();
        }

        protected virtual string NoDoneReplica() => noDoneReplicas[GetRandomIndex(noDoneReplicas)];

        protected int GetRandomIndex(string[] arr) => Random.Range(0, arr.Length);
        
        public override void SaveData() => _startStage = stage;

        public override void ResetData()
        {
            stage = _startStage;

            UnsubscribeListeners();
        }
    }
}