using UnityEngine;

public class QuestButton : MonoBehaviour
{
    public Quest quest;

    public void CheckStageQuest()
    {
        if (quest != null)
            quest.CheckStage();
    }

    public void CloseDialogWindow() => EventHandler.OnDialogueWindowShow?.Invoke(false);
}