using UnityEngine;

namespace Scripts.Level.Quests.Dialogue_Classes
{
    public abstract class Conversation : ScriptableObject
    {
        public Questor Questor { get; set; }

        [Header("Required Level")]
        [Tooltip("Требуемый уровень зла в мире (выше указанного)"), Min(0)]
        [SerializeField] protected int minLevel;

        [Tooltip("Требуемый уровень зла в мире (ниже указанного)"), Min(0)]
        [SerializeField] protected int maxLevel = 10;

        [Tooltip("Диалоги, завершающие текущий")]
        [SerializeField] protected Conversation[] _finalConversations;

        [Tooltip("Диалоги, которые нужно выполнить для активации текущего")]
        [SerializeField] protected Conversation[] _requiredConversations;

        public virtual void Initialize() =>
                EventHandler.OnConversationPassed.AddListener(CheckPassedConversation);

        public abstract void Talk();
        public abstract bool IsAvailable();

        public bool IsRequiredConversationsPassed()
        {
            foreach (var c in _requiredConversations)
                if (!c.IsAvailable())
                    return false;

            return true;
        }

        public bool IsRelevantLevel(int evilLevel) =>
                evilLevel >= minLevel && evilLevel <= maxLevel;

        public void CheckPassedConversation(Conversation conversation)
        {
            foreach (var c in _finalConversations)
                if (c == conversation)
                {
                    PassConversation();
                    EventHandler.OnConversationPassed.RemoveListener(CheckPassedConversation);
                }
        }

        public abstract void PassConversation();
        public abstract string GetDescription();
        public abstract void SaveData();
        public abstract void ResetData();
    }
}