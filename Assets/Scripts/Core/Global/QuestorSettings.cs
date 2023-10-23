// using EditorExtension;
// using UnityEngine;
// using UnityEngine.Serialization;
//
// public class QuestorSettings : MonoBehaviour
// {
// #if UNITY_EDITOR
//
//     [SerializeField] private Questor[] _questors;
//
//     [FormerlySerializedAs("_confirmDeletion")] [SerializeField]
//     private bool _confirmation;
//
//     [Button("Save Quests")]
//     public void SaveButton()
//     {
//         foreach (var questor in _questors)
//             questor.SaveButton();
//     }
//
//     [Button("Save Quests With Replacement")]
//     public void SaveWithReplacementButton()
//     {
//         if (_confirmation)
//         {
//             foreach (var questor in _questors)
//                 questor.SaveWithReplacementButton();
//
//             _confirmation = false;
//         }
//     }
//
//     [Button("Load Quests")]
//     public void LoadButton()
//     {
//         foreach (var questor in _questors)
//             questor.LoadButton();
//     }
//
//     [Button("Delete Quests")]
//     public void DeleteButton()
//     {
//         if (_confirmation)
//         {
//             foreach (var questor in _questors)
//                 questor.DeleteButton();
//
//             _confirmation = false;
//         }
//     }
//
// #endif
// }