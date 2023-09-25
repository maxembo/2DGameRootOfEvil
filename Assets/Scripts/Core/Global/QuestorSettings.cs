using EditorExtension;
using UnityEngine;

public class QuestorSettings : MonoBehaviour
{
#if UNITY_EDITOR

    [SerializeField] private Questor[] _questors;

    [Button("Save Quests")]
    public void SaveButton()
    {
        foreach (var questor in _questors)
            questor.SaveButton();
    }

    [Button("Save Quests With Replacement")]
    public void SaveWithReplacementButton()
    {
        foreach (var questor in _questors)
            questor.SaveWithReplacementButton();
    }

    [Button("Load Quests")]
    public void LoadButton()
    {
        foreach (var questor in _questors)
            questor.LoadButton();
    }

    [Button("Delete Quests")]
    public void DeleteButton()
    {
        foreach (var questor in _questors)
            questor.DeleteButton();
    }

#endif
}