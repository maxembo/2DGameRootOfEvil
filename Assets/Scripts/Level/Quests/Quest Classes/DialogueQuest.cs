using Scripts.Level.Quests.Dialogue_Classes;
using UnityEngine;

namespace Scripts.Level.Quests.Quest_Classes
{
    [CreateAssetMenu(fileName = "Dialogue Quest", menuName = "Quests/Dialogue Quest", order = 0)]
    public class DialogueQuest : Quest
    {
        [SerializeField] private Dialogue[] _dialogs;

        public override void Initialize()
        {
            base.Initialize();
            
            EventHandler.OnDialogPassed2.AddListener(CheckCondition);
        }
        
        protected override bool SomeCondition()
        {
            foreach (var dialog in _dialogs)
                if (dialog.IsAvailable()) return false;

            return true;
        }
    }
}