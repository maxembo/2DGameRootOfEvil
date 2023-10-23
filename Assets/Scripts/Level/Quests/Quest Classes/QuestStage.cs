using System;
using UnityEngine;

namespace Scripts.Level.Quests.Quest_Classes
{
    [Serializable] public struct QuestStage<T>
    {
        public T obj;
        [Tooltip("Стадия, устанавливаемая товарищескому квесту")]
        public Quest.QuestStages achievableStage;
    }
}