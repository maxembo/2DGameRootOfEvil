using UnityEngine;

namespace Scripts.Core.Global
{
    public class GameState : MonoBehaviour
    {
        public GameStates CurrentState { get; private set; }
        public GameObject pausePanel;

        public static GameState instance;

        void Start()
        {
            if (instance == null) instance = this;
            else if (instance == this) Destroy(gameObject);
        }

        private void Update()
        {
            if (InputFunctions.GetEscapeButton_Up())
                ChangeMode();
        }

        private void ChangeMode()
        {
            if (CurrentState == GameStates.Playing) Pause();
            else Play();
        }

        public void Pause()
        {
            pausePanel.SetActive(true);
            EventHandler.OnDialogueWindowShow2?.Invoke(false);
            Time.timeScale = 0;

            CurrentState = GameStates.Paused;
        }

        public void Play()
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1;
        
            CurrentState = GameStates.Playing;
        }
    }

    public enum GameStates
    {
        Playing,
        Paused
    }
}