using UnityEngine;

namespace Scripts.Level.Quests.Dialogue_Classes
{
    public class TalkButton : MonoBehaviour
    {
        public TalkButton(Conversation convers) { conversation = convers; }
        
        public Conversation conversation;

        public void InitializeConversation(Conversation conversation)
        {
            this.conversation = conversation;
            
            conversation.Initialize();
        }
        
        public void Talk() { if (conversation != null) conversation.Talk(); }

        public void CloseDialogWindow() => EventHandler.OnDialogueWindowShow2?.Invoke(false);
    }
}