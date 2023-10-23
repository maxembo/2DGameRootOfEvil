using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Level.Quests.Dialogue_Classes
{
    public class DialogueWindow2 : MonoBehaviour
    {
        private Questor _currentQuestor;
        [SerializeField] private GameObject _dialogPanel;
        [SerializeField] private ContentSizeFitter _replicaContent;
        [SerializeField] private ContentSizeFitter _dialogContent;
        [SerializeField] private TextMeshProUGUI _dialogText;
        [SerializeField] private Button _dialogButton;
        [SerializeField] private Button _closeDialogButton;
        [SerializeField] private bool _isShowed;

        private void Start()
        {
            EventHandler.OnDialogueWindowShow2.AddListener(WindowSetActive);
            EventHandler.OnShowDialogs.AddListener(ShowDialogs);
            EventHandler.OnReplicaSay2.AddListener(SayReplica);
        }

        public void WindowSetActive(bool value)///////////////refactoring
        {
            _dialogPanel.SetActive(value);
            _isShowed = value;

            if (value == false)
                CloseDialogWindow();
        }

        public void ShowDialogs(Conversation[] dialogs, Questor questor, int evilLevel)
        {
            _currentQuestor = questor;

            ClearPanel(_dialogContent.transform);
            
            Instantiate(_dialogText, _dialogContent.transform).text = "Поговорить:";
            
            foreach (Conversation dialog in dialogs)
                if (dialog.IsAvailable() && dialog.IsRelevantLevel(_currentQuestor.GetCurrentEvilLevel()))
                    SetDialogButton(dialog);

            SetQuitDialogButton();
        }

        private void SetDialogButton(Conversation convers)
        {
            var clone = SetButton(_dialogButton, _dialogContent.transform);
            clone.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = convers.GetDescription();
            clone.GetComponent<TalkButton>().InitializeConversation(convers);
        }

        private void SetQuitDialogButton()
        {
            SetButton(_closeDialogButton, _dialogContent.transform).
                    transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Закончить разговор.";
        }

        private Button SetButton(Button button, Transform parent) => Instantiate(button, parent: parent);

        public void SayReplica(Questor questor, string replica)
        {
            if (_currentQuestor != null && questor == _currentQuestor && _isShowed)
                Instantiate(_dialogText, _replicaContent.transform).text = replica;
        }

        public void ClearPanel(Transform panel)
        {
            Transform[] allChildren = panel.GetComponentsInChildren<Transform>();

            for (int i = 1; i < allChildren.Length; i++)
                Destroy(allChildren[i].gameObject);
        }

        public void CloseDialogWindow()
        {
            ClearPanel(_dialogContent.transform);
            ClearPanel(_replicaContent.transform);
        }
    }
}