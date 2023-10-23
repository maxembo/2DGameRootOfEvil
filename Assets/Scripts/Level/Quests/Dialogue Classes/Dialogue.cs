using UnityEngine;

namespace Scripts.Level.Quests.Dialogue_Classes
{
    [CreateAssetMenu(fileName = "New Dialog", menuName = "Dialogs/Dialog", order = 0)]
    public class Dialogue : Conversation
    {
        #region Vars

        [SerializeField] private string _description;
        [SerializeField] private bool _isAvailable;

        [Header("Quest Replicas")]
        [TextArea(2, 4)]
        public string[] questReplicas;

        private bool _availability;
        private string[] _questReplicas;

        #endregion

        #region Base Methods

        public override void Initialize() => SaveData();

        [ContextMenu("Save Data")] public override void SaveData()
        {
            _availability = IsAvailable();
            _questReplicas = questReplicas;
        }

        [ContextMenu("Reset Data")] public override void ResetData()
        {
            _isAvailable = _availability;
            _questReplicas = null;
        }

        public override void Talk()
        {
            CheckCondition();
            SayAndRemoveReplica(ref _questReplicas);
        }

        private void SayAndRemoveReplica(ref string[] replicas)
        {
            if (!_isAvailable) return;

            EventHandler.OnReplicaSay2?.Invoke(Questor, replicas[0]);
            RemoveReplica(ref replicas, 0);
        }

        private void CheckCondition()
        {
            if (_questReplicas.Length != 0)
            {
                _isAvailable = true;

                return;
            }

            _isAvailable = false;
            
            EventHandler.OnDialogPassed2?.Invoke();
        }

        public override bool IsAvailable() => _isAvailable && IsRequiredConversationsPassed();
        
        public void SetAvailability(bool value) => _isAvailable = value;

        public override string GetDescription() => _description;

        public override void PassConversation() => EventHandler.OnConversationPassed?.Invoke(this);////////
        
        #endregion

        #region Other Methods

        private void RemoveReplica(ref string[] arr, int index)
        {
            string[] newArr = new string[arr.Length - 1];

            int j = 0;

            for (int i = 0; i < arr.Length; i++)
                if (i != index)
                {
                    newArr[j] = arr[i];
                    j++;
                }

            arr = newArr;
        }

        #endregion
    }
}