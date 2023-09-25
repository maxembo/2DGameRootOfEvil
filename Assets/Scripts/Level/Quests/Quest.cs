using System;
using UnityEngine;

public abstract class Quest : ScriptableObject
{
    #region Vars

    [Header("Quest")] 
    public string questName;
    public string description;

    [Tooltip("На случай, если квест имеет чисто диалоговый характер и необходимо, чтобы он не зачитывался")]
    public bool isCounted = true;
    
    [NonSerialized] public Questor questor;

    [Header("Quest States")] public QuestStages stage;

    public enum QuestStages
    {
        NotAvailable,
        NotStarted,
        Progressing,
        Completed,
        Passed
    };

    [Header("Base Replicas")] [TextArea(2, 4)]
    public string givingReplica;

    [TextArea(2, 4)] public string[] noDoneReplicas;
    [TextArea(2, 4)] public string completeReplica;
    
    [Header("Requirements Quest Launch")] 
    [Tooltip("Требуемый уровень зла в мире (выше указанного)")]
    public int minLevel;

    [Tooltip("Требуемый уровень зла в мире (ниже указанного)")]
    public int maxLevel = 10;

    [Tooltip("Эти квесты должны быть начаты, для того чтобы квест стал доступным")]
    public Quest[] requiredQuests_start;

    [Tooltip("Эти квесты должны быть выполнены, для того чтобы квест стал доступным")]
    public Quest[] requiredQuests_complete;

    [Tooltip("Эти квесты должны быть завершены, для того чтобы квест стал доступным")]
    public Quest[] requiredQuests_pass;

    [Header("Change other Quests")] [Tooltip("Квесты, завершающие данный квест после их начала")]
    public Quest[] passQuests_start;

    [Tooltip("Квесты, завершающие данный квест после их выполнения")]
    public Quest[] passQuests_complete;

    [Tooltip("Квесты, завершающие данный квест после их завершения")]
    public Quest[] passQuests_pass;

    [Header("Make Available after this Quests")] [Tooltip("Делает доступными эти квесты, после старта этого")]
    public Quest[] availableQuests_start;

    [Tooltip("Делает доступными эти квесты, после выполнения этого")]
    public Quest[] availableQuests_complete;

    [Tooltip("Делает доступными эти квесты, после завершения этого")]
    public Quest[] availableQuests_pass;
    
    [Header("Complete action in another quest")]
    [Tooltip("Завершает одно квестовое действие у этих квестов, после завершения этого")]
    public ItemQuest[] _completingAction_itemQuests;

    #endregion

    #region Base Methods

    public bool QuestAvailability(int evilLevel) =>
        CheckRequiredQuests() && CheckRequiredLevel(evilLevel) && CheckRequiredStage();

    public void SetQuestor(Questor questor) => this.questor = questor;

    public void SetStage(QuestStages stage) => this.stage = stage;

    public void CheckQuest()
    {
        switch (stage)
        {
            case QuestStages.NotStarted:
                StartQuest();
                break;
            case QuestStages.Progressing:
                ProgressingQuest();
                break;
            case QuestStages.Completed:
                PassQuest();
                break;
        }
    }

    public void StartQuest()
    {
        MakeAvailableQuests(availableQuests_start);

        EventHandler.OnQuestStart?.Invoke(this);
        EventHandler.OnReplicaSay?.Invoke(questor, givingReplica);

        EventHandler.OnQuestStart.AddListener(PassingQuestAfterStart);
        EventHandler.OnQuestComplete.AddListener(PassingQuestAfterComplete);
        EventHandler.OnDialogPassed.AddListener(PassingQuestAfterPass);

        SetStage(QuestStages.Progressing);
    }

    public void ProgressingQuest()
    {
        if (SomeCondition())
        {
            CompleteQuest();

            PassQuest();
        }
        else NoDone();
    }

    public void CompleteQuest()
    {
        EventHandler.OnQuestComplete?.Invoke(this);

        MakeAvailableQuests(availableQuests_complete);

        SetStage(QuestStages.Completed);
    }

    public void PassQuest()
    {
        EventHandler.OnReplicaSay?.Invoke(questor, completeReplica);

        questor.SetSmileSprite();

        if (isCounted) EventHandler.OnQuestPassed?.Invoke(this);
        else EventHandler.OnDialogPassed?.Invoke(this);

        MakeAvailableQuests(availableQuests_pass);

        SetStage(QuestStages.Passed);
        
        ReduceActionInOtherQuest();
    }
    
    public void ReduceActionInOtherQuest()
    {
        if (_completingAction_itemQuests != null)
            foreach (var quest in _completingAction_itemQuests)
                quest.ReduceAction();
    }
    public abstract void ReduceAction();

    public abstract bool SomeCondition();
    
    public abstract void NoDone();

    #endregion

    #region Passing Quests from other Quest

    public void PassingQuestAfterStart(Quest quest) => PassingQuest(quest, passQuests_start);

    public void PassingQuestAfterComplete(Quest quest) => PassingQuest(quest, passQuests_complete);

    public void PassingQuestAfterPass(Quest quest) => PassingQuest(quest, passQuests_pass);

    public void PassingQuest(Quest quest, Quest[] quests)
    {
        foreach (var passingQuest in quests)
            if (quest.name == passingQuest.name)
            {
                PassQuest();

                break;
            }
    }

    #endregion

    #region Other Methods

    public int GetRandomIndex(string[] arr) => UnityEngine.Random.Range(0, arr.Length);
    
    private bool CheckRequiredQuests()
    {
        if (
            IsMeetsTheRequirements(requiredQuests_start, QuestStages.Progressing) &&
            IsMeetsTheRequirements(requiredQuests_complete, QuestStages.Completed) &&
            IsMeetsTheRequirements(requiredQuests_pass, QuestStages.Passed)
        ) return true;

        return false;
    }

    private bool CheckRequiredLevel(int evilLevel) => evilLevel >= minLevel && evilLevel <= maxLevel;

    private bool CheckRequiredStage() => stage != QuestStages.NotAvailable && stage != QuestStages.Passed;

    private void MakeAvailableQuests(Quest[] quests)
    {
        foreach (var quest in quests)
            quest.SetStage(QuestStages.NotStarted);
    }

    private bool IsMeetsTheRequirements(Quest[] quests, QuestStages requiredStage)
    {
        foreach (var quest in quests)
            if (quest.stage != requiredStage)
                return false;

        return true;
    }

    #endregion
}