using UnityEngine;

namespace Scripts.Character.Classes
{
    public class Warrior : Character
    {
        [Min(1)] public int attackDamage;

        public override void UsePrimaryAction()
        {
            if (holdedItem != null && holdedItem.TryGetComponent(out Weapon weapon))
                MultiplyDamage(weapon);

            base.UsePrimaryAction();
        }

        private void MultiplyDamage(Weapon weapon) => weapon.DamageFactor = attackDamage;
    }
}