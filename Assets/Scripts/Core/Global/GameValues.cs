using InputData;
using UnityEngine;
using UnityEngine.UI;

public class GameValues : MonoBehaviour
{
    public GameModes gameMode;
    public GameObject pausePanel;

    private GameModes _currentGameMode;

    private void Start()
    {
        ChangeGameMode(GameModes.Playing);

        EventHandler.OnDialogueWindowShow.AddListener(SetPause);
    }

    private void Update()
    {
        if (_currentGameMode != gameMode)
            ChangeGameMode(gameMode);

        if (InputFunctions.GetEscapeButton_Up())
            PausePanelActive();
    }

    public void PausePanelActive() ///
    {
        switch (_currentGameMode)
        {
            case GameModes.Playing:
                pausePanel.SetActive(true);
                ChangeGameMode(GameModes.Pause);
                EventHandler.OnGameModeChanged?.Invoke(_currentGameMode);
                break;
            case GameModes.Pause:
                pausePanel.SetActive(false);
                ChangeGameMode(GameModes.Playing);
                EventHandler.OnDialogueWindowShow?.Invoke(false);
                EventHandler.OnGameModeChanged?.Invoke(_currentGameMode);
                break;
        }
    }

    public void ChangeGameMode(GameModes mode)
    {
        switch (mode)
        {
            case GameModes.Playing:
                Time.timeScale = 1;
                break;
            case GameModes.Pause:
                Time.timeScale = 0;
                break;
        }

        gameMode = mode;
        _currentGameMode = mode;

        EventHandler.OnGameModeChanged?.Invoke(_currentGameMode);
    }

    public void SetPause(bool b)
    {
        if (b) ChangeGameMode(GameModes.Pause);
        else ChangeGameMode(GameModes.Playing);
    }

    public GameModes GetGameMode() => _currentGameMode;
}

public enum GameModes
{
    Playing,
    Pause
}