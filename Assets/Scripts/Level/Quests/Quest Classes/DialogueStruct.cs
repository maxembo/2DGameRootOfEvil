using System;
using Scripts.Level.Quests.Dialogue_Classes;
using UnityEngine;

namespace Scripts.Level.Quests.Quest_Classes
{
    [Serializable] public struct DialogueStruct
    {
            [Tooltip("Товарищеский диалог")]
            public Dialogue companionableDialog;
            [Tooltip("Стадия текущего квеста, при достижении которой, " +
                     "товарищеский квест меняет свою доступность")]
            public Quest.QuestStages achievableStage;
            [Tooltip("Доступность, устанавливаемая товарищескому квесту")]
            public bool availability;
    }
}