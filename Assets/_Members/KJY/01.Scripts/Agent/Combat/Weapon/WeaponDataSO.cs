using UnityEngine;

namespace _Members.KJY._01.Scripts.Agent.Combat.Weapon
{
    [CreateAssetMenu(fileName = "Weapon Data", menuName = "KJY/Weapon/Weapon Data", order = 0)]
    public class WeaponDataSO : ScriptableObject
    {
        [field: SerializeField] public string WeaponName { get; private set; }
        [field: SerializeField] public AbstractWeapon Weapon { get; private set; }
        [field: SerializeField] public WeaponType WeaponType{ get; private set; }
    }
}