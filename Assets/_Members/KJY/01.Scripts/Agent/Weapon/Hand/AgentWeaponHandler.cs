using _Members.KJY._01.Scripts.Object.Rotate;
using _TevLib.ModuleSystem;

namespace _Members.KJY._01.Scripts.Agent.Weapon.Hand
{
    public class AgentWeaponHandler : RotateObject , IModule
    {
        public AbstractWeapon CurrentWeapon {get; private set;}

        public void Init(AbstractWeapon initWeapon)
        {
            SetWeapon(initWeapon);
        }
        
        public void SetWeapon(AbstractWeapon newWeapon)
        {
            AbstractWeapon newWeaponGo = Instantiate(newWeapon,transform);
            CurrentWeapon = newWeaponGo;
            CurrentWeapon.Init();
        }

        public void Attack(AttackData attackData)
        {
            if (!attackData.IsAttacking) return;
            CurrentWeapon.Attack(attackData);
        }
    }
}