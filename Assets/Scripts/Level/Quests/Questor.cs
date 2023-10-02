using UnityEngine;
using InputData;
using UnityEditor;
using System;
using System.IO;
using EditorExtension;
using UnityEngine.Serialization;

public class Questor : MonoBehaviour
{
    [FormerlySerializedAs("_gameValues")] [Header("Game Values Manager")] [SerializeField]
    private GameState gameState;

    [Header("Questor")] public string questorName;

    [Header("Sprites")] [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _smileNPC;
    [SerializeField] private Sprite _sadNPC;

    [Header("EvilLevel")] [SerializeField] private EvilLevelCounter _evilLevelCounter;

    [Header("Dialogue")] [TextArea(2, 4)] [SerializeField]
    private string _textGreeting = "Приветствую!";

    [Header("Quest")] [SerializeField] private Quest[] _quests;

    private bool _dialogueReady;

    private void Start()
    {
        SetNullableFields();

        SetSadSprite();

        EventHandler.OnEvilLevelChanged.AddListener(CheckEvilLevel);
    }

    private void Update()
    {
        if (InputFunctions.GetKeyF_Up() && _dialogueReady)
            Dialogue();
    }

    private void SetNullableFields()
    {
        if (gameState == null)
            gameState = FindObjectOfType<GameState>();

        if (_spriteRenderer == null)
            _spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();

        if (_evilLevelCounter == null)
            _evilLevelCounter = FindObjectOfType<EvilLevelCounter>();
    }

    public void Dialogue()
    {
        if (gameState.CurrentState == GameStates.Paused) return;

        EventHandler.OnDialogueWindowShow?.Invoke(true);
        EventHandler.OnShowQuests?.Invoke
            (_quests, this, _evilLevelCounter.GetCurrentEvilLevel());
        EventHandler.OnReplicaSay?.Invoke(this, _textGreeting);
    }

    public void SetSmileSprite() => _spriteRenderer.sprite = _smileNPC;

    public void SetSadSprite() => _spriteRenderer.sprite = _sadNPC;

    public void CheckEvilLevel(int evilLevel)
    {
        if (evilLevel >= 9)
            SetSadSprite();
    }

    #region Trigger

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (gameState.CurrentState == GameStates.Playing && collision.CompareTag("Player"))
            _dialogueReady = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (gameState.CurrentState == GameStates.Playing && collision.CompareTag("Player"))
            _dialogueReady = false;
    }

    #endregion

#if UNITY_EDITOR

    #region Save & Load data Methods

    [Button("Save Quests")]
    public void SaveButton()
    {
        SaveQuests();

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    [Button("Save Quests With Replacement")]
    public void SaveWithReplacementButton()
    {
        DeleteQuests();
        SaveQuests();

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.SetDirty(this);
    }

    [Button("Load Quests")]
    public void LoadButton()
    {
        foreach (var quest in _quests)
        {
            string path = $"Assets/Scriptable Objects/Quests/Quests Data/{questorName}";

            if (AssetDatabase.Contains(quest))
            {
                path += $"/{quest.questName}_copy.asset";

                CopyFields(AssetDatabase.LoadAssetAtPath(path, quest.GetType()), quest);
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    [Button("Delete Quests")]
    public void DeleteButton()
    {
        DeleteQuests();

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void SaveQuests(bool withReplacement = false)
    {
        foreach (var quest in _quests)
        {
            string path = "Assets/Scriptable Objects/Quests/Quests Data";

            // ��������� ������������� �����.
            if (!AssetDatabase.IsValidFolder(path + "/" + questorName))
                AssetDatabase.CreateFolder(path, questorName);

            path = $"Assets/Scriptable Objects/Quests/Quests Data/{questorName}/";

            string assetName = $"{quest.questName}_copy.asset";

            // ��������� ������������� ������.
            if (File.Exists(path + assetName))
                throw new Exception($"Asset {assetName} already exists.");

            if (quest is ItemQuest)
            {
                var newQuest = ScriptableObject.CreateInstance<ItemQuest>();

                SaveAsset(quest, newQuest, path + assetName);
            }
            else if (quest is DialogueQuest)
            {
                var newQuest = ScriptableObject.CreateInstance<DialogueQuest>();

                SaveAsset(quest, newQuest, path + assetName);
            }
        }
    }


    public void DeleteQuests()
    {
        foreach (var quest in _quests)
        {
            string path = $"Assets/Scriptable Objects/Quests/Quests Data/{questorName}/";

            // ��������� ������������� �����.
            if (!AssetDatabase.IsValidFolder(path))
                throw new Exception($"Folder {path} is not valid.");

            string assetName = $"{quest.questName}_copy.asset";

            DeleteAsset(path + assetName);
        }
    }

    private void DeleteAsset(string path) => AssetDatabase.DeleteAsset(path);

    private void SaveAsset(UnityEngine.Object oldObj, UnityEngine.Object newObj, string path)
    {
        CopyFields(oldObj, newObj);

        // ������� �����.
        AssetDatabase.CreateAsset(newObj, path);
    }

    private void CopyFields(UnityEngine.Object oldObj, UnityEngine.Object newObj)
    {
        // ������� ��� ���� ������.
        var oldFields = oldObj.GetType().GetFields();
        var newFields = newObj.GetType().GetFields();

        for (int i = 0; i < oldFields.Length; i++)
            // ������������� ��� ������ ������ newObj � ���� newFields[i] �������� fields[i] ������ obj.
            newFields[i].SetValue(newObj, oldFields[i].GetValue(oldObj));
    }

    #endregion

#endif
}