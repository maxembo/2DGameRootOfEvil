using System;
using UnityEngine;

namespace Scripts.Level.Quests.Quest_Classes
{
    [Serializable] public struct QuestStruct<T>
    {
        public QuestStage<T> questStage;
        [Tooltip("Стадия, устанавливаемая товарищескому квесту")]
        public Quest.QuestStages assignableStage;
    }
}