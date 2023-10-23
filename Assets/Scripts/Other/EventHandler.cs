using Scripts.Level.Quests.Dialogue_Classes;
using Scripts.Level.Quests.Quest_Classes;
using UnityEngine.Events;

public static class EventHandler
{
    public static UnityEvent OnPlayerDeath = new UnityEvent();
    public static UnityEvent OnEnemyKilled = new UnityEvent();
    public static UnityEvent<int> OnEvilLevelChange = new UnityEvent<int>();
    
    #region Quest and Dialog Events

    public static UnityEvent<Conversation[], Questor, int> OnShowDialogs = new();
    public static UnityEvent<Questor, string> OnReplicaSay2 = new();
    public static UnityEvent<bool> OnDialogueWindowShow2 = new();
    public static UnityEvent<Quest> OnQuestProgressing = new();
    public static UnityEvent<Quest> OnQuestCompleted = new();
    public static UnityEvent<Quest> OnQuestPassed2 = new();
    public static UnityEvent<DialogueQuest> OnDialogueQuestPassed = new();
    public static UnityEvent OnDialogPassed2 = new();
    public static UnityEvent<Conversation> OnConversationPassed = new();

    #endregion
}