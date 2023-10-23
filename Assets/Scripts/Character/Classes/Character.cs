using Scripts.Core.Common;
using Scripts.Core.Global;
using UnityEngine;

namespace Scripts.Character.Classes
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Character : MonoBehaviour
    {
        [SerializeField] private bool _isPlayer;
        [SerializeField][Range(0, 10)] private int _movementSpeed;

        public TransportableItem holdedItem;

        private Rigidbody2D _rbody;

        private void Awake() => _rbody = GetComponent<Rigidbody2D>();

        #region Character

        public virtual void UsePrimaryAction()
        {
            if (
                    holdedItem != null &&
                    holdedItem.TryGetComponent(out UsableItem usedItem)
            )
                usedItem.PrimaryAction();
        }

        public void UseSecondaryAction()
        {
            if (
                    holdedItem != null &&
                    holdedItem.TryGetComponent(out UsableItem usedItem)
            )
                usedItem.SecondaryAction();
        }

        public void MoveTo(Vector2 movementDirection)
        {
            float speed = _movementSpeed * GlobalConstants.CoefMovementSpeed;

            ExecuteCommand(new MoveCommand(_rbody, movementDirection * speed));
        }

        public void StopMove()
        {
            ExecuteCommand(new MoveCommand(_rbody, Vector2.zero * 0));
        }

        public void RotateByAngle(Transform obj, float angle)
        {
            ExecuteCommandByValue(new RotationCommand(obj), angle);
        }

        public void Death()
        {
            if (_isPlayer)
            {
                EventHandler.OnPlayerDeath?.Invoke();
                
                PlayerTransition.Transiting(transform, GlobalConstants.TavernInside);
            }
            else
            {
                EventHandler.OnEnemyKilled?.Invoke();

                Destroy(gameObject);
            }
        }

        #endregion

        #region Common

        private void ExecuteCommand(Command command) => command.Execute();

        private void ExecuteCommandByValue(Command command, float value) => command.ExecuteByValue(value);

        #endregion
    }
}