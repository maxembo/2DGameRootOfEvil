using UnityEngine;

namespace Scripts.Core.Common
{
    public class MoveCommand : Command
    {
        public MoveCommand(Rigidbody2D actor, Vector2 moveDirection)
        {
            _actor = actor;
            _moveDirection = moveDirection;
        }

        private Rigidbody2D _actor;
        private Vector2 _moveDirection;

        public override void Execute()
        {
            _actor.velocity = _moveDirection;
        }
    }
}