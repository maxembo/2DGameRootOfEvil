using Scripts.Level.Quests.Quest_Classes;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class EvilLevelCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _evilLevelCounter;
    [SerializeField] private PostProcessVolume _postProcessingVol;
    [SerializeField] private Transform _balk;
    [SerializeField] private string _textScales;
    [SerializeField] [Range(0, 10)] private int _evilLevel;
    [SerializeField] [Range(0, 10)] private int _minAchievedLevel;

    private void Start()
    {
        _minAchievedLevel = _evilLevel;
        
        EventHandler.OnDialogueQuestPassed.AddListener(CheckResetDialog);
        EventHandler.OnQuestPassed2.AddListener(EvilLevelDown);
        EventHandler.OnEnemyKilled.AddListener(EvilLevelUp);
        EventHandler.OnEvilLevelChange.Invoke(_evilLevel);
    }

    public int GetCurrentEvilLevel() => _evilLevel;

    public void ResetLevel() => _evilLevel = _minAchievedLevel;

    private void EvilLevelUp()
    {
        if (_evilLevel < 10) SetEvilLevel(_evilLevel + 1);
    }
    
    private void EvilLevelDown(Quest quest)
    {
        if (_evilLevel > 0) SetEvilLevel(_evilLevel - 1);
    
        CheckMinLevel();
    }

    private void SetEvilLevel(int newLevel)
    {
        _evilLevel = newLevel;

        SetScalesAngle();

        _evilLevelCounter.text = $"{_textScales}{_evilLevel}";

        EventHandler.OnEvilLevelChange.Invoke(_evilLevel);
    }

    private void CheckMinLevel()
    {
        if (_evilLevel < _minAchievedLevel)
            _minAchievedLevel = _evilLevel;
    }

    private void SetScalesAngle()
    {
        if (_postProcessingVol != null)
            _postProcessingVol.weight = (float)_evilLevel / 10;

        _balk.transform.rotation = Quaternion.Euler(0, 0, (5 - _evilLevel) * 5);
    }
    
    private void CheckResetDialog(DialogueQuest quest)
    {
        if (quest.questName == "Reset Level")
            ResetLevel();
    }

    #region EDITOR_MODE

    private int _oldEvilLevel;

    private void Update() => CheckLevel();

    private void CheckLevel()
    {
        if (_evilLevel != _oldEvilLevel)
        {
            _oldEvilLevel = _evilLevel;

            SetEvilLevel(_evilLevel);

            CheckMinLevel();
        }
    }

    #endregion
}