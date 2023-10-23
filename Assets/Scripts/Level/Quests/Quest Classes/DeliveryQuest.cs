using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Level.Quests.Quest_Classes
{
    [CreateAssetMenu(fileName = "Delivery Quest", menuName = "Quests/Delivery Quest", order = 0)]
    public class DeliveryQuest : Quest
    {
        [Tooltip("Количество квестовых предметов для его выполнения")]
        public int itemCount;

        [Tooltip("Предмет, необходимый для выполнения квеста")]
        public UsableItem questItem;

        [Tooltip("Собранные квестовые предметы")]
        public List<UsableItem> questItemsCollections;

        #region Base Methods
        
        protected override bool SomeCondition() => questItemsCollections.Count >= itemCount;

        #endregion
        
        // private void CopyFields<T>(T oldObj, T newObj) //
        // {
        //     var oldFields = oldObj.GetType().GetFields();
        //     var newFields = newObj.GetType().GetFields();
        //
        //     for (int i = 0; i < oldFields.Length; i++)
        //         newFields[i].SetValue(newObj, oldFields[i].GetValue(oldObj));
        // }

        public void AddItem(UsableItem item)
        {
            if (!IsContains(item) && IsNeeded(item)) 
                questItemsCollections.Add(item);
        }

        public void RemoveItem(UsableItem item)
        {
            if (IsContains(item) && IsNeeded(item))
                questItemsCollections.Remove(item);
        }

        public bool IsContains(UsableItem item) => questItemsCollections.Contains(item);

        public bool IsNeeded(UsableItem item) => item.nameItem == questItem.nameItem;
        
        protected override string NoDoneReplica()
        {
            if (questItem != null)
                return noDoneReplicas[GetRandomIndex(noDoneReplicas)] +
                       $"\n[{questItem.nameItem}: {itemCount - questItemsCollections.Count}]";

            return noDoneReplicas[GetRandomIndex(noDoneReplicas)];
        }
        
        public override void ResetData()
        {
            base.ResetData();
            
            questItemsCollections = null;
        }
    }
}
