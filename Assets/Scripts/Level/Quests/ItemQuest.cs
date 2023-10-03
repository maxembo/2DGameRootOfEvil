using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Quest", menuName = "Quests/ItemQuest", order = 0)]
public class ItemQuest : Quest
{
    #region Vars

    [Tooltip("Количество квестовых действий для его выполнения")]
    public int questActions;

    [Tooltip("Предмет, необходимый для выполнения квеста")]
    public UsableItem questItem;

    [Tooltip("Собранные квестовые предметы")]
    public List<UsableItem> questItemsCollections;

    #endregion

    #region Base Methods

    public override void ReduceAction()
    {
        if (questActions > 0)
            questActions--;
    }

    public override bool SomeCondition() => questItemsCollections.Count == questActions;

    public override void NoDone() => EventHandler.OnReplicaSay?.Invoke(questor, NoDoneReplica());

    #endregion

    #region Other Methods

    public void AddItem(UsableItem item)
    {
        if (!questItemsCollections.Contains(item) && item.nameItem == questItem.nameItem)
            questItemsCollections.Add(item);
    }

    public void RemoveItem(UsableItem item)
    {
        if (questItemsCollections.Contains(item) && item.nameItem == questItem.nameItem)
            questItemsCollections.Remove(item);
    }
    
    private string NoDoneReplica()
    {
        if (questItem != null)
            return noDoneReplicas[GetRandomIndex(noDoneReplicas)] +
                   $"\n[{questItem.nameItem}: {questActions - questItemsCollections.Count}]";

        return noDoneReplicas[GetRandomIndex(noDoneReplicas)];
    }

    #endregion
}