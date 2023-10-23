using UnityEngine;

namespace Scripts.Level.Quests.Quest_Classes
{
    [CreateAssetMenu(fileName = "Perform Action Quest", menuName = "Quests/Perform Action Quest", order = 0)]
    public class PerformActionQuest : Quest
    {
        [SerializeField] private int _startCountActions = 1;

        private int _countAction;
        
        public override void Initialize()
        {
           base.Initialize();

           _countAction = _startCountActions;
        }

        public void PerformAction()
        {
            _countAction--;
            
            CheckCondition();
        }
        
        protected override bool SomeCondition() => _countAction == 0;
    }
}