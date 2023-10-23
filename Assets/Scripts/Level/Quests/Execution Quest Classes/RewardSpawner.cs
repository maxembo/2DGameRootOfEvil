using UnityEngine;

namespace Scripts.Level.Quests.Execution_Quest_Classes
{
    public class RewardSpawner : MonoBehaviour
    {
        [Tooltip("Награда за действие (Оставьте пустым если нет)")]
        [SerializeField] private UsableItem _reward;

        [Tooltip("Кол-во наград за действие (Оставьте пустым если нет)")]
        [SerializeField] [Min(0)] private int _rewardCount;

        [Tooltip("Позиция спавна награды (Оставьте пустым если нет)")]
        [SerializeField] private Transform _rewardPosition;

        public void GiveReward()
        {
            if (_reward != null && _rewardCount > 0)
            {
                GiveReward(_reward);

                _rewardCount--;
            }
        }

        public void GiveReward(UsableItem item) => Instantiate(item, GetRewardPos(), Quaternion.identity);

        private Vector2 GetRewardPos() => _rewardPosition.transform.position;
    }
}