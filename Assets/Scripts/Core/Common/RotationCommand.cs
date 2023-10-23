using UnityEngine;

namespace Scripts.Core.Common
{
    public class RotationCommand : Command
    {
        public RotationCommand(Transform rotatableObj)
        {
            _rotatableObj = rotatableObj;
        }

        private Transform _rotatableObj;

        public override void Execute()
        {

        }

        public override void ExecuteByValue(float angle)
        {
            _rotatableObj.localRotation = Quaternion.Euler(0f, angle, 0f);
        }
    }
}